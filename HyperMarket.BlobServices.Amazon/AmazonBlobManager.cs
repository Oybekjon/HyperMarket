using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices.Amazon
{
    //TODO: Put guards in this class
    public class AmazonBlobManager : BlobManager
    {
        private readonly string bucketLocation;
        private readonly string awsKey;
        private readonly string awsSecretKey;
        private readonly int blockSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketLocation">
        /// Expect location like "us-east-2"
        /// </param>
        /// <param name="awsKey"></param>
        /// <param name="awsSecretKey"></param>
        /// <param name="globalPrefix"></param>
        public AmazonBlobManager(
            string bucketLocation,
            string awsKey,
            string awsSecretKey,
            string globalPrefix
        )
            : base(globalPrefix)
        {
            this.bucketLocation = bucketLocation ?? throw new ArgumentNullException(nameof(bucketLocation));
            this.awsKey = awsKey ?? throw new ArgumentNullException(nameof(awsKey));
            this.awsSecretKey = awsSecretKey ?? throw new ArgumentNullException(nameof(awsSecretKey));

            blockSize = (int)Math.Pow(2, 20) * 5;
        }

        public override Task<Stream> ContinuousUpload(string path)
        {
            Guard.NotNullOrEmpty(path, nameof(path));
            var (container, key) = ProcessWithGlobalPrefix(path, true);
            var client = GetClient(bucketLocation);
            var stream = new AmazonContinuousUploadStream(client, container, key, blockSize);
            return Task.FromResult((Stream)stream);
        }

        public override async Task CopyAsync(string source, string target)
        {
            Guard.NotNullOrEmpty(source, nameof(source));
            Guard.NotNullOrEmpty(target, nameof(target));
            var client = GetClient();
            var (sourceBucket, sourceKey) = ProcessWithGlobalPrefix(source, true);
            var (targetBucket, targetKey) = ProcessWithGlobalPrefix(target, true);
            var request = new CopyObjectRequest
            {
                SourceBucket = sourceBucket,
                SourceKey = sourceKey,
                DestinationBucket = targetBucket,
                DestinationKey = targetKey
            };
            await client.CopyObjectAsync(request);
        }

        public override async Task CreateFolder(string path)
        {
            if (!path.EndsWith("/"))
                path += "/";
            await UploadAsync(path, new byte[0]);
        }

        public override async Task DeleteAsync(string path)
        {
            Guard.NotNullOrEmpty(path, nameof(path));
            var (bucket, fileName) = ProcessWithGlobalPrefix(path, true);
            var delRequest = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = fileName
            };
            var client = GetClient();
            await client.DeleteObjectAsync(delRequest);
        }

        public override async Task<int> DeleteFolderAsync(string path)
        {
            Guard.NotNullOrEmpty(path, nameof(path));
            var client = GetClient();
            var (bucket, key) = ProcessWithGlobalPrefix(path, true);
            if (string.IsNullOrWhiteSpace(key))
            {
                throw ErrorHelper.NotAllowed("You cannot delete bucket. It is not allowed");
            }

            var structure = await GetFolderStructure(bucket, key);
            var request = new DeleteObjectsRequest
            {
                BucketName = bucket,
                Objects = structure
            };

            await client.DeleteObjectsAsync(request);

            return structure.Count;
        }

        public override async Task<IReadOnlyCollection<EntryInfo>> GetContentsAsync(string path, bool foldersOnly)
        {
            Guard.NotNullOrEmpty(path, nameof(path));
            var (container, key) = ProcessWithGlobalPrefix(path, false);
            if (string.IsNullOrWhiteSpace(container))
            {
                return await GetBuckets();
            }
            else
            {
                var client = GetClient(bucketLocation);
                if (!string.IsNullOrWhiteSpace(key) && !key.EndsWith('/'))
                {
                    key += '/';
                }

                var req = new ListObjectsV2Request
                {
                    BucketName = container,
                    Delimiter = "/",
                    Prefix = key
                };

                var result = await client.ListObjectsV2Async(req);
                var folders = ParseCommonPrefixes(container, result.CommonPrefixes);
                var files = result.S3Objects
                    .Where(x => x.Key != key)
                    .Select(y => new FileInfo
                    {
                        FullPath = combineLocal(y.BucketName, y.Key),
                        Path = ExtractName(y.Key),
                        Size = y.Size
                    });

                var entries = foldersOnly ? folders : folders.Union(files).ToList();
                RemovePrefixes(entries);
                return entries;

                string combineLocal(string bucket, string name)
                {
                    return Combine(bucket, name);
                }
            }
        }

        public override Task<string> GetDownloadUrl(string path)
        {
            return GetDownloadUrl(path, DefaultUrlTimeout);
        }

        public override Task<string> GetDownloadUrl(string path, TimeSpan expiresIn)
        {
            Guard.NotNullOrEmpty(path, nameof(path));
            var (container, key) = ProcessWithGlobalPrefix(path, true);
            var client = GetClient();
            var props = new Dictionary<string, object>();
            var url = client.GeneratePreSignedURL(container, key, DateTime.UtcNow.Add(expiresIn), props);
            return Task.FromResult(url);
        }

        public async override Task<Stream> GetFileAsync(string path)
        {
            Guard.NotNullOrEmpty(path, nameof(path));
            var (container, key) = ProcessWithGlobalPrefix(path, true);
            var client = GetClient();
            var props = new Dictionary<string, object>();
            var result = await client.GetObjectStreamAsync(container, key, props);
            return result;
        }

        public override async Task MoveAsync(string source, string target)
        {
            await CopyAsync(source, target);
            await DeleteAsync(source);
        }

        public override async Task UploadAsync(string path, byte[] content)
        {
            Guard.NotNull(content, nameof(content));
            await UploadAsync(path, content.ToStream());
        }

        public override async Task UploadAsync(string path, Stream content)
        {
            Guard.NotNull(path, nameof(path));
            Guard.NotNull(content, nameof(content));
            var (bucket, key) = ProcessWithGlobalPrefix(path, true);
            var request = new PutObjectRequest
            {
                Key = key,
                BucketName = bucket,
                InputStream = content
            };
            var client = GetClient();
            await client.PutObjectAsync(request);
        }

        public override async Task UploadAsync(string path, string localFilePath)
        {
            using var file = File.Open(localFilePath, FileMode.Open);
            await UploadAsync(path, file);
        }

        private IAmazonS3 GetClient(string region = null)
        {
            region = !string.IsNullOrWhiteSpace(region) ? region : bucketLocation;
            var regionEndpoint = RegionEndpoint.GetBySystemName(region);
            return new AmazonS3Client(awsKey, awsSecretKey, new AmazonS3Config
            {
                SignatureVersion = "4",
                RegionEndpoint = regionEndpoint,
                SignatureMethod = SigningAlgorithm.HmacSHA256
            });
        }

        private List<EntryInfo> ParseCommonPrefixes(string container, IReadOnlyCollection<string> commonPrefixes)
        {
            return commonPrefixes.Select(x => (EntryInfo)new FolderInfo
            {
                FullPath = combineLocal(x),
                Path = ExtractName(x)
            }).ToList();

            string combineLocal(string str)
            {
                return Combine(container, str);
            }
        }

        private static string ExtractName(string fullPath)
        {
            if (fullPath.EndsWith('/'))
            {
                fullPath = fullPath[0..^1];
            }
            var lastIndex = fullPath.LastIndexOf('/');
            return fullPath.Substring(lastIndex + 1);
        }

        private async Task<IReadOnlyCollection<AmazonFolderInfo>> GetBuckets()
        {
            // list buckets
            var buckets = await GetClient(null).ListBucketsAsync();
            var bucketList = new List<AmazonFolderInfo>();
            foreach (var item in buckets.Buckets)
            {
                bucketList.Add(new AmazonFolderInfo
                {
                    FullPath = item.BucketName,
                    Path = item.BucketName
                });
            }

            bucketList
                .AsParallel()
                .ForAll(
                    x => AsyncHelpers.RunSync(() => SetBucketLocation(x))
                );

            return bucketList;
        }

        private async Task SetBucketLocation(AmazonFolderInfo x)
        {
            var response = await GetClient(null).GetBucketLocationAsync(x.Path);
            x.Location = response.Location;
        }

        private async Task DeleteObjects(string bucket, List<KeyVersion> keys)
        {
            var deleteObjectsRequest = new DeleteObjectsRequest
            {
                Objects = keys,
                BucketName = bucket
            };
            var client = GetClient();
            await client.DeleteObjectsAsync(deleteObjectsRequest);
        }

        private async Task<List<KeyVersion>> GetFolderStructure(string bucket, string key)
        {
            if (key != null && !key.EndsWith("/"))
            {
                key += "/";
            }

            var request = new ListObjectsV2Request
            {
                BucketName = bucket,
                Prefix = key,
                Delimiter = "/"
            };
            var client = GetClient();
            var result = await client.ListObjectsV2Async(request);
            var list = result.S3Objects.Select(x => new KeyVersion
            {
                Key = x.Key
            }).ToList();
            var bag = new ConcurrentBag<KeyVersion>();

            result.CommonPrefixes.AsParallel().ForAll(x => AsyncHelpers.RunSync(() => getStructure(x)));

            list.AddRange(bag);
            return list;

            async Task getStructure(string path)
            {
                var subResults = await GetFolderStructure(bucket, path);
                bag.AddRange(subResults);
            }
        }
    }
}
