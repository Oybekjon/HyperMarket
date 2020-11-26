using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices.Amazon
{
    internal class AmazonContinuousUploadStream : Stream
    {
        private readonly IAmazonS3 amazonS3;
        private readonly string bucket;
        private readonly string key;
        private readonly long blockSize;
        private readonly object lockHandle = new object();
        private readonly Dictionary<int, string> eTags;
        private MemoryStream container;
        private int index;
        private bool isDisposed;
        private InitiateMultipartUploadResponse multipartUpload;

        public AmazonContinuousUploadStream(
            IAmazonS3 amazonS3,
            string bucket,
            string key,
            long blockSize)
        {
            this.amazonS3 = amazonS3 ?? throw new ArgumentNullException(nameof(amazonS3));
            this.bucket = bucket ?? throw new ArgumentNullException(nameof(bucket));
            this.key = key ?? throw new ArgumentNullException(nameof(key));
            if (blockSize < 5 * 1024 * 1024)
            {
                throw new ArgumentException("Block size must be minimum of 5MB i.e. 5 * 2 ^ 20 bytes");
            }

            this.blockSize = blockSize;
            container = new MemoryStream();
            eTags = new Dictionary<int, string>();
            index = 1;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (isDisposed)
                throw new InvalidOperationException();
            container.Write(buffer, offset, count);
            
            while (container.Length > blockSize)
            {
                AsyncHelpers.RunSync(() => Push());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(AmazonContinuousUploadStream));
            isDisposed = true;
            AsyncHelpers.RunSync(() => Push());

            var req = new CompleteMultipartUploadRequest
            {
                BucketName = bucket,
                Key = key,
                PartETags = eTags.Select(x => new PartETag(x.Key, x.Value)).ToList(),
                UploadId = multipartUpload.UploadId
            };

            AsyncHelpers.RunSync(() => amazonS3.CompleteMultipartUploadAsync(req));
        }

        private async Task Push()
        {
            if (multipartUpload == null)
            {
                lock (lockHandle)
                {
                    if (multipartUpload == null)
                    {
                        multipartUpload = AsyncHelpers.RunSync(() => amazonS3.InitiateMultipartUploadAsync(bucket, key));
                    }
                }
            }

            container.Position = 0;
            var min = Math.Min(blockSize, container.Length);
            var bytes = new byte[min];
            if (min == 0)
                return;
            container.Read(bytes, 0, bytes.Length);

            var remSize = container.Length - bytes.Length;
            var rem = new byte[remSize];
            if (remSize > 0)
            {
                container.Read(rem, 0, (int)remSize);
            }
            container = new MemoryStream();
            if (remSize > 0)
            {
                container.Write(rem, 0, (int)remSize);
            }
            var index = this.index;
            this.index++;

            var block = new MemoryStream(bytes)
            {
                Position = 0
            };

            var req = new UploadPartRequest
            {
                BucketName = bucket,
                Key = key,
                PartNumber = index,
                UploadId = multipartUpload.UploadId,
                PartSize = min,
                InputStream = block
            };

            var uplResult = await amazonS3.UploadPartAsync(req);
            eTags[uplResult.PartNumber] = uplResult.ETag;
            block.Dispose();
        }
    }
}
