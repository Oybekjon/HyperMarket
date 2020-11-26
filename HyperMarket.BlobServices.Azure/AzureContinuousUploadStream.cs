using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices.Azure
{
    internal class AzureContinuousUploadStream : Stream
    {
        private MemoryStream container;
        private readonly CloudBlockBlob target;
        private readonly long blockSize;
        private readonly List<string> keys;
        private int index;
        private bool isDisposed;

        public AzureContinuousUploadStream(CloudBlockBlob target, long blockSize)
        {
            Guard.NotNull(target, nameof(target));
            this.blockSize = blockSize;
            this.target = target;
            container = new MemoryStream();
            index = 0;
            keys = new List<string>();
            isDisposed = false;
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
                Push();
            }
        }

        public override void Close()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(AzureContinuousUploadStream));
            isDisposed = true;
            Push();
            target.PutBlockList(keys);
        }

        private void Push()
        {
            container.Position = 0;
            var min = Math.Min(blockSize, container.Length);
            var bytes = new byte[min];
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
            var key = Convert.ToBase64String(BitConverter.GetBytes(index));
            var block = new MemoryStream(bytes)
            {
                Position = 0
            };
            target.PutBlock(key, block, new Checksum(Checksum(bytes)));
            keys.Add(key);
        }

        private string Checksum(byte[] values)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new MemoryStream(values))
                {
                    var result = md5.ComputeHash(stream);
                    return Convert.ToBase64String(result);
                }
            }
        }
    }
}
