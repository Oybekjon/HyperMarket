using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace HyperMarket.BlobServices.Azure
{
    public class AzureBlobManager : BlobManager
    {
        private readonly string connectionString;

        /// <summary>
        /// Global prefix is Windows Azure specific prefix to indicate the root folder to start copying.
        /// Useful to isolate folders withing the storage.
        /// </summary>
        /// <param name="connectionString">
        /// Connection string to WABS
        /// </param>
        /// <param name="globalPrefix">
        /// Windows Azure specific prefix
        /// </param>
        public AzureBlobManager(string connectionString, string globalPrefix)
            : base(globalPrefix)
        {
            Guard.NotNullOrEmpty(connectionString, "connectionString");
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Deletes an object from windows Azure. Key is a storage key, e.g. demostore/somefolder/hello.jpg
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(string key)
        {
            key = PrependGlobalPrefix(key);
            var file = await GetBlobReference(key, true);
            if (!file.Exists())
            {
                throw new Exception($"File {key} not found");
            }
            file.Delete();
        }

        public override async Task<IReadOnlyCollection<EntryInfo>> GetContentsAsync(string path, bool foldersOnly)
        {
            var (containerName, fileName) = ProcessWithGlobalPrefix(path, false);
            var account = GetCloudStorageAccount();
            var client = account.CreateCloudBlobClient();
            if (string.IsNullOrWhiteSpace(containerName))
            {
                var conts = client.ListContainers().Select(x => (EntryInfo)new FolderInfo
                {
                    FullPath = $"{x.Name}",
                    Path = x.Name
                }).ToList();
                return conts;
            }
            else
            {
                var container = client.GetContainerReference(containerName);
                var list = (List<IListBlobItem>)null;
                var result = new List<EntryInfo>();
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    list = container.ListBlobs().ToList();
                }
                else
                {
                    var dir = container.GetDirectoryReference(fileName);
                    list = dir.ListBlobs().ToList();
                    // directory level query
                }

                var dirs = list.OfType<CloudBlobDirectory>().ToList();
                var blobs = list.OfType<CloudBlockBlob>().ToList();

                for (var i = 0; i < dirs.Count; i++)
                {
                    var current = dirs[i];
                    var name = current.Prefix.Trim('/');
                    var lastPart = name.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    var dir = new FolderInfo
                    {
                        FullPath = $"{containerName}/{name}/",
                        Path = lastPart
                    };

                    result.Add(dir);
                }

                if (!foldersOnly)
                {
                    for (var i = 0; i < blobs.Count; i++)
                    {
                        var currentBlob = blobs[i];

                        var lastName = currentBlob.Name.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                        var created = currentBlob.Properties.Created;
                        result.Add(new FileInfo
                        {
                            FullPath = $"{containerName}/{currentBlob.Name}",
                            Path = lastName,
                            Size = currentBlob.Properties.Length,
                            DateCreated = !created.HasValue ? (DateTime?)null : created.Value.DateTime.ToUniversalTime()
                        });
                    }
                }

                RemovePrefixes(result);
                return await Task.FromResult(result);
            }
        }

        public override async Task<int> DeleteFolderAsync(string path)
        {
            Guard.NotNullOrEmpty(path, "path");
            path = PrependGlobalPrefix(path);
            SplitKey(path, out var containerName, out var fileName, false);
            var account = GetCloudStorageAccount();
            var client = account.CreateCloudBlobClient();
            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new Exception("You must indicate at least a container");
            }
            var entries = (List<EntryInfo>)null;
            var container = client.GetContainerReference(containerName);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                entries = GetChildren(containerName, () => container.ListBlobs().ToList());
            }
            else
            {
                var folder = container.GetDirectoryReference(fileName);
                entries = GetChildren(containerName, () => folder.ListBlobs().ToList());
            }
            var allFiles = CollectFiles(entries);
            var files = allFiles.Select(x =>
            {
                SplitKey(x.FullPath, out var cntName, out var fl, true);
                return container.GetBlobReference(fl);
            }).ToList();

            var count = 0;
            files.AsParallel().ForAll(x =>
            {
                if (x.DeleteIfExists())
                {
                    Interlocked.Increment(ref count);
                }
            });

            return await Task.FromResult(count);
        }

        public override async Task<Stream> GetFileAsync(string path)
        {
            path = PrependGlobalPrefix(path);
            var file = await GetBlobReference(path, true);
            if (!file.Exists())
            {
                throw new Exception($"File {path} not found");
            }
            var ms = new MemoryStream();
            file.DownloadToStream(ms);
            ms.Position = 0;
            return ms;
        }

        public override async Task UploadAsync(string key, byte[] content)
        {
            Guard.NotNull(content, nameof(content));
            await UploadAsync(key, x => x.UploadFromByteArray(content, 0, content.Length));
        }

        public override async Task UploadAsync(string key, Stream content)
        {
            Guard.NotNull(content, nameof(content));
            await UploadAsync(key, x => x.UploadFromStream(content));
        }

        public override async Task UploadAsync(string key, string localFilePath)
        {
            await UploadAsync(key, x => x.UploadFromFile(localFilePath));
        }

        public override async Task<string> GetDownloadUrl(string path)
        {
            return await GetDownloadUrl(path, DefaultUrlTimeout);
        }

        public override async Task<string> GetDownloadUrl(string path, TimeSpan expiresIn)
        {
            path = PrependGlobalPrefix(path);
            var blob = await GetBlobReference(path, true);

            var policy = new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = DateTime.UtcNow.Add(expiresIn),
                Permissions = SharedAccessBlobPermissions.Read
            };
            var signature = blob.GetSharedAccessSignature(policy);
            return blob.Uri + signature;
        }

        public override async Task MoveAsync(string source, string target)
        {
            await Copy(source, target, true);
        }

        public override async Task CopyAsync(string source, string target)
        {
            await Copy(source, target, false);
        }

        public override async Task CreateFolder(string path)
        {
            Guard.NotNullOrEmpty(path, nameof(path));
            path = PrependGlobalPrefix(path);
            var filePath = path.TrimEnd('/') + $"/{emptyFileName}";
            var blob = await GetBlobReference(filePath, false);
            if (!blob.Exists())
            {
                blob.UploadFromByteArray(new byte[0], 0, 0);
            }
        }

        private async Task Copy(string source, string target, bool deleteSource)
        {
            target = PrependGlobalPrefix(target);
            source = PrependGlobalPrefix(source);
            var @ref = await GetBlobReference(target, false);
            var sourceFile = await GetBlobReference(source, true);
            @ref.StartCopy(sourceFile);
            if (deleteSource)
                sourceFile.Delete();
        }

        public override async Task<Stream> ContinuousUpload(string path)
        {
            path = PrependGlobalPrefix(path);
            var file = await GetBlobReference(path, false);
            return new AzureContinuousUploadStream(file, 512 * 1024);
        }

        private async Task UploadAsync(string key, Action<CloudBlockBlob> upload)
        {
            key = PrependGlobalPrefix(key);
            var file = await GetBlobReference(key, false);
            if (file.Exists())
            {
                throw new Exception("File with the same name already exists");
            }
            upload(file);
        }

        private async Task<CloudBlockBlob> GetBlobReference(string key, bool throwIfContainerAbsent)
        {
            Guard.NotNullOrEmpty(key, nameof(key));
            SplitKey(key, out var containerName, out var filename, true);
            var account = GetCloudStorageAccount();
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);
            if (throwIfContainerAbsent && !container.Exists())
            {
                throw new Exception($"Container {container} not found");
            }
            await container.CreateIfNotExistsAsync();
            var file = container.GetBlockBlobReference(filename);
            return file;
        }

        private CloudStorageAccount GetCloudStorageAccount()
        {
            if (CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                return storageAccount;
            }

            throw new FormatException("ConnectionString is invalid");
        }







        private List<EntryInfo> CollectFiles(List<EntryInfo> entries)
        {
            var folders = entries.Where(x => x.IsFolder).ToList();
            var files = new List<EntryInfo>();
            foreach (var folder in folders)
            {
                var children = CollectFiles(((FolderInfo)folder).Children);
                files.AddRange(children);
            }
            var currentFiles = entries.Where(x => !x.IsFolder).ToList();
            files.AddRange(currentFiles);
            return files;
        }

        private List<EntryInfo> GetChildren(string container, Func<List<IListBlobItem>> getBlobs)
        {

            var result = new List<EntryInfo>();
            var objects = getBlobs();
            var dirs = objects.OfType<CloudBlobDirectory>().ToList();
            var blobs = objects.OfType<CloudBlockBlob>().ToList();

            for (var i = 0; i < dirs.Count; i++)
            {
                var current = dirs[i];
                var name = current.Prefix.Trim('/');
                var lastPart = name.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                var dir = new FolderInfo
                {
                    FullPath = $"{container}/{name}/",
                    Path = lastPart
                };
                dir.Children = GetChildren(container, () => current.ListBlobs().ToList());
                result.Add(dir);
            }

            for (var i = 0; i < blobs.Count; i++)
            {
                var currentBlob = blobs[i];
                currentBlob.FetchAttributes();

                var lastName = currentBlob.Name.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                var created = currentBlob.Properties.Created;
                result.Add(new FileInfo
                {
                    FullPath = $"{container}/{currentBlob.Name}",
                    Path = lastName,
                    Size = currentBlob.Properties.Length,
                    DateCreated = !created.HasValue ? (DateTime?)null : created.Value.DateTime.ToUniversalTime()
                });
            }
            return result;
        }
    }
}
