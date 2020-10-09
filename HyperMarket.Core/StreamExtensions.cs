using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
namespace HyperMarket {
    public static class StreamExtensions {
        public static Byte[] ReadChunk(this Stream source, Int64 chunkSize, Int32 chunkIndex) {
            if (source == null)
                throw new NullReferenceException();
            if (!source.CanSeek || !source.CanRead)
                throw new NotSupportedException();
            var realSize = Math.Min(source.Length - chunkIndex * chunkSize, chunkSize);
            var buffer = new Byte[realSize];
            var iniPos = source.Position;
            source.Position = chunkSize * chunkIndex;
            source.Read(buffer, 0, Convert.ToInt32(realSize));
            source.Position = iniPos;
            return buffer;
        }
        public static MemoryStream ToStream(this Byte[] data) {
            return new MemoryStream(data);
        }
        public static MemoryStream CopyToMemory(this Stream source, MemoryStream destination) {
            if (source == null)
                throw new NullReferenceException();
            if (destination == null)
                throw ErrorHelper.ArgNull("destination");
            var iniPosition = -1L;
            if (source.CanSeek)
                iniPosition = source.Position;
            source.CopyTo(destination);
            destination.Position = 0;
            if (source.CanSeek)
                source.Position = iniPosition;
            return destination;
        }
        public static MemoryStream CopyToMemory(this Stream source) {
            return source.CopyToMemory(new MemoryStream());
        }
        public static String ToBase64(this Byte[] data) {
            if (data == null)
                throw new NullReferenceException();
            return Convert.ToBase64String(data);
        }
        public static void AppendText(this FileStream source, String text) {
            WriteToStream(source, text);
        }
        public static void OverwriteWithText(this FileStream source, String text) {
            WriteToStream(source, text, true);
        }
        public static String ReadAsText(this Stream source) {
            return source.ReadAsText(Encoding.UTF8);
        }
        public static String ReadAsText(this Stream source, Encoding encoding) {
            if (source == null)
                throw new NullReferenceException();
            Guard.NotNull(encoding, "encoding");
            return encoding.GetString(source.Read());
        }
        public static Byte[] Read(this Stream source, Int32 count) {
            if (source == null)
                throw new NullReferenceException();
            var position = 0L;
            if (source.CanSeek) {
                position = source.Position;
                source.Position = 0;
            }
            var bytes = new Byte[count];
            var totalRead = source.Read(bytes, 0, count);
            if (totalRead < count)
                bytes = bytes.Take(totalRead).ToArray();
            if (source.CanSeek)
                source.Position = position;
            return bytes;
        }
        public static Byte[] Read(this Stream source) {
            if (source == null)
                throw new NullReferenceException();
            var position = 0L;
            if (source.CanSeek) {
                position = source.Position;
                source.Position = 0;
            }
            var bytes = (Byte[])null;
            if (!source.CanSeek)
                bytes = NonSeekable(source).ToArray();
            else {
                bytes = new Byte[source.Length];
                source.Read(bytes, 0, bytes.Length);
            }
            if (source.CanSeek)
                source.Position = position;
            return bytes;
        }
        public static void SafeDispose(this Stream source) {
            if (source == null) return;
            source.Close();
            source.Dispose();
        }
        public static void WriteText(this Stream source, String text) {
            if (source == null)
                throw new NullReferenceException();
            if (text == null)
                throw ErrorHelper.ArgNull("text");
            var bytes = Encoding.UTF8.GetBytes(text);
            source.Write(bytes);
        }
        public static void Write(this Stream source, Byte[] data) {
            source.Write(data, 0, data.Length);
        }
        public static Boolean IsPlainText(this Stream str) {
            var buffer = new Byte[1024];
            var position = (Int64?)null;
            if (str.CanSeek) {
                position = str.Position;
            }
            var count = str.Read(buffer, 0, 1024);
            if (str.CanSeek && position.HasValue)
                str.Position = position.Value;
            return buffer.Take(count).ToArray().IsPlainText();
        }

        public static Boolean IsPlainText(this Byte[] data) {
            Guard.NotNull(data, "data");
            var count = 0;
            for (int nPosition = 0; nPosition < data.Length; nPosition++) {
                int a = data[nPosition];

                if (!(a >= 0 && a <= 127)) {
                    count++;
                }
                var percentage = (Double)count / (Double)data.Length;
                if (nPosition > 5 && percentage > .1)
                    return false;
            }
            return true;
        }
        private static List<Byte> NonSeekable(Stream source) {
            var bytes = new List<Byte>();
            var readCount = 0;
            var lastRead = 0;
            var bufferSize = 4096;
            do {
                var buff = new Byte[bufferSize];
                lastRead = source.Read(buff, 0, bufferSize);
                bytes.AddRange(buff.Take(lastRead));
                readCount += lastRead;
            } while (lastRead == bufferSize);
            return bytes;
        }
        private static void WriteToStream(this FileStream source, String text, Boolean truncate = false) {
            if (source == null)
                throw new NullReferenceException();
            if (text == null)
                throw ErrorHelper.ArgNull("text");
            if (truncate)
                source.SetLength(0);
            else
                source.Position = source.Length;
            var bytes = Encoding.UTF8.GetBytes(text);
            source.Write(bytes, 0, bytes.Length);
        }
    }
}