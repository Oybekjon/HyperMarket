using System;
using System.IO;
using System.Text;
namespace HyperMarket {
    public static class StringExtensions {
        public static Stream ToStream(this String source) {
            if (source == null)
                throw new NullReferenceException();
            return source.ToByte().ToStream();
        }
        public static String[] SplitToLines(this String value, Boolean removeEmptyLines = false) {
            if (value == null)
                throw new NullReferenceException();
            return value.Replace("\r", "").Split(new[] { '\n' }, removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        public static String ToFilenameSafeString(this String value) {
            if (value == null)
                throw new NullReferenceException();
            if (value.Length == 0)
                return value;
            var chars = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(value);
            foreach (var @char in chars) 
                sb.Replace(@char, '-');
            return sb.ToString();
        }
    }
}