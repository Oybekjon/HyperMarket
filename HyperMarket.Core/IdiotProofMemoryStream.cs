using System;
using System.IO;
namespace HyperMarket {
    /// <summary>
    ///     This class is used in cases with methods when the streams
    ///     for some strange reasons are disposed.
    ///     This stream is resistant to such kind of disposition and will contain
    ///     the data even after calling Dispose method.
    /// </summary>
    public class IdiotProofMemoryStream : Stream {
        private MemoryStream InternalStream;
        public IdiotProofMemoryStream() {
            InternalStream = new MemoryStream();
        }
        public IdiotProofMemoryStream(Byte[] buffer) {
            InternalStream = new MemoryStream(buffer);
        }
        public override Boolean CanRead {
            get { return InternalStream.CanRead; }
        }
        public override Boolean CanSeek {
            get { return InternalStream.CanSeek; }
        }
        public override Boolean CanWrite {
            get { return InternalStream.CanWrite; }
        }
        public override void Flush() {
            InternalStream.Flush();
        }
        public override Int64 Length {
            get { return InternalStream.Length; }
        }
        public override Int64 Position {
            get { return InternalStream.Position; }
            set { InternalStream.Position = value; }
        }
        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count) {
            return InternalStream.Read(buffer, offset, count);
        }
        public override Int64 Seek(Int64 offset, SeekOrigin origin) {
            return InternalStream.Seek(offset, origin);
        }
        public override void SetLength(Int64 value) {
            InternalStream.SetLength(value);
        }
        public override void Write(Byte[] buffer, Int32 offset, Int32 count) {
            InternalStream.Write(buffer, offset, count);
        }
        protected override void Dispose(Boolean disposing) { }
        public override void Close() { }
        public MemoryStream GetUnderlyingStream() {
            return InternalStream;
        }
    }
}