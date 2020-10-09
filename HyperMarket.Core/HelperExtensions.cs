using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data;
namespace HyperMarket {
    public static class HelperExtensions {
        /// <summary>
        /// Safer ToString() method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String AsString(this Object value) {
            return value == null ? null : value.ToString();
        }
        public static String AsString(this Byte[] value) {
            return value.AsString(Encoding.UTF8);
        }
        public static String AsString(this Byte[] value, Encoding encoding) {
            if (value == null)
                throw null;
            Guard.NotNull(encoding, "encoding");
            return encoding.GetString(value);
        }
        public static String AsString(this Object value, String format) {
            if (value == null)
                return null;
            return String.Format(String.Format("{{0:{0}}}", format), value);
        }
        public static String AsString(this Object value, String format, IFormatProvider formatProvider) {
            Guard.NotNull(formatProvider, "formatProvider");
            if (value == null)
                return null;
            return String.Format(formatProvider, String.Format("{{0:{0}}}", format), value);
        }
        /// <summary>
        /// Serializes Exception into XElement
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static XElement AsXmlElement(this Exception ex) {
            if (ex == null)
                return null;
            var element = new XElement("Exception");
            if (!String.IsNullOrWhiteSpace(ex.Message))
                element.Add(new XElement("Message", ex.Message));
            if (!String.IsNullOrWhiteSpace(ex.Source))
                element.Add(new XElement("Source", ex.Source));
            if (!String.IsNullOrWhiteSpace(ex.StackTrace))
                element.Add(new XElement("StackTrace", ex.StackTrace));
            if (ex.InnerException != null)
                element.Add(new XElement("InnerException", ex.InnerException.AsXmlElement()));
            return element;
        }
        /// <summary>
        /// Converts this value to int
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;int&gt;</returns>
        public static Int32? AsInt(this String value) {
            if (!String.IsNullOrEmpty(value)) {
                Int32 k;
                if (Int32.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out k))
                    return k;
            }
            return default(Int32?);
        }
        /// <summary>
        /// Converts this value to long
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;long&gt;</returns>
        public static Int64? AsLongInt(this String value) {
            if (!String.IsNullOrEmpty(value)) {
                Int64 k;
                if (Int64.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out k))
                    return k;
            }
            return default(Int64?);
        }
        /// <summary>
        /// Converts this value to double
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;double&gt;</returns>
        public static Double? AsDouble(this String value) {
            if (!String.IsNullOrEmpty(value)) {
                Double k;
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out k)) {
                    return k;
                }
            }
            return default(Double?);
        }
        /// <summary>
        /// Converts this value to decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;decimal&gt;</returns>
        public static Decimal? AsDecimal(this String value) {
            if (!String.IsNullOrEmpty(value)) {
                Decimal k;
                if (Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out k))
                    return k;
            }
            return default(Decimal?);
        }
        /// <summary>
        /// Converts this value to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;DateTime&gt;</returns>
        public static DateTime? AsDateTime(this String value) {
            if (!String.IsNullOrEmpty(value)) {
                DateTime d;
                if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                    return d;
            }
            return default(DateTime?);
        }
        /// <summary>
        /// Converts this value to bool, if parsing fails, will be returned <paramref name="@default"/>
        /// </summary>
        /// <param name="value">value to convert (true|false|on|off insensetive to case)</param>
        /// <returns>Returns Boolean representation of the string value</returns>
        public static Boolean? AsBoolean(this String value) {
            if (!String.IsNullOrEmpty(value)) {
                Boolean d;
                if (Boolean.TryParse(value, out d))
                    return d;
                if (String.Equals(value, "on", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (String.Equals(value, "off", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return default(Boolean?);
        }
        /// <summary>
        /// Converts the string into the Guid, and returns null if the conversion fails
        /// </summary>
        /// <param name="value">String vaue to convert.</param>
        /// <returns>Conversion result</returns>
        public static Guid? AsGuid(this String value) {
            Guid gVal;
            if (Guid.TryParse(value, out gVal))
                return gVal;
            return null;
        }
        /// <summary>
        /// Converts this value to int, if parsing fails, will be returned <paramref name="default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="default">the default value for fail case</param>
        /// <returns>Returns int</returns>
        public static Int32 AsInt(this String value, Int32 @default) {
            if (!String.IsNullOrEmpty(value)) {
                Int32 k;
                if (Int32.TryParse(value, out k))
                    return k;
            }
            return @default;
        }
        /// <summary>
        /// Converts this value to long, if parsing fails, will be returned <paramref name="default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="default">the default value for fail case</param>
        /// <returns>Returns int</returns>
        public static Int64 AsLongInt(this String value, Int64 @default) {
            if (!String.IsNullOrEmpty(value)) {
                Int64 k;
                if (Int64.TryParse(value, out k))
                    return k;
            }
            return @default;
        }
        /// <summary>
        /// Converts this value to double, if parsing fails, will be returned <paramref name="default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="default">the default value for fail case</param>
        /// <returns>Returns double</returns>
        public static Double AsDouble(this String value, Double @default) {
            if (!String.IsNullOrEmpty(value)) {
                Double k;
                if (Double.TryParse(value, out k))
                    return k;
            }
            return @default;
        }
        /// <summary>
        /// Converts this value to decimal, if parsing fails, will be returned <paramref name="@default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="@default">the default value for fail case</param>
        /// <returns>Returns decimal</returns>
        public static Decimal AsDecimal(this String value, Decimal @default) {
            if (!String.IsNullOrEmpty(value)) {
                Decimal k;
                if (Decimal.TryParse(value, out k))
                    return k;
            }
            return @default;
        }
        /// <summary>
        /// Converts this value to DateTime, if parsing fails, will be returned <paramref name="@default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="@default">the default value for fail case</param>
        /// <returns>Returns DateTime</returns>
        public static DateTime AsDateTime(this String value, DateTime @default) {
            if (!String.IsNullOrEmpty(value)) {
                DateTime d;
                if (DateTime.TryParse(value, out d))
                    return d;
            }
            return @default;
        }
        /// <summary>
        /// Converts this value to bool, if parsing fails, will be returned <paramref name="@default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="@default">the default value for fail case</param>
        /// <returns>Returns bool</returns>
        public static Boolean AsBoolean(this String value, Boolean @default) {
            if (!String.IsNullOrEmpty(value)) {
                Boolean d;
                if (Boolean.TryParse(value, out d))
                    return d;
                if (String.Equals(value, "on", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (String.Equals(value, "off", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return @default;
        }
        /// <summary>
        /// Attemtps to convert the provided string into the Guid and returns the provided value if the conversion fails.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="default">Default value to return if the conversion fails.</param>
        /// <returns></returns>
        public static Guid AsGuid(this String value, Guid @default) {
            Guid res;
            if (Guid.TryParse(value, out res))
                return res;
            return @default;
        }

        public static String Format(this String format, params Object[] args) {
            if (format == null)
                throw ErrorHelper.ArgNull("format");
            return String.Format(format, args);
        }
        public static String ToText(this Byte[] input) {
            if (input == null)
                throw ErrorHelper.ArgNull("input");
            return Encoding.UTF8.GetString(input);
        }
        public static Byte[] ToByte(this String input) {
            return input.ToByte(Encoding.UTF8);
        }
        public static Byte[] ToByte(this String input, Encoding encoding) {
            Guard.NotNull(input, "input");
            Guard.NotNull(encoding, "encoding");
            return encoding.GetBytes(input);
        }
        public static ImageFormatType GetImageFormat(this Stream data) {
            Guard.NotNull(data, "data");
            if (!data.CanSeek || !data.CanRead)
                throw ErrorHelper.NotSupported("This operation is not supported for the streams that cannot be read or written");

            var buffer = new Byte[4];
            var current = data.Position;
            data.Position = 0;
            var countRead = data.Read(buffer, 0, 4);
            data.Position = current;
            return buffer.GetImageFormat();
        }
        public static ImageFormatType GetImageFormat(this Byte[] bytes) {
            Guard.NotNull(bytes, "bytes");
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new Byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new Byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new Byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new Byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new Byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormatType.Bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormatType.Gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormatType.Png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormatType.Tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormatType.Tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormatType.Jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormatType.Jpeg;

            return ImageFormatType.Unknown;
        }
        public static Boolean TryCast<T>(this Object obj, out T target) {
            try {
                target = (T)obj;
                return true;
            } catch { }
            try {
                target = (T)Convert.ChangeType(obj, typeof(T));
                return true;
            } catch { }
            target = default(T);
            return false;
        }
        public static T CastEnum<T>(this Object value) where T : struct {
            if (value == null)
                throw ErrorHelper.InvalidArgument("You passed null");
            if (value.GetType() == typeof(Int32))
                return (T)value;
            if (value.GetType() == typeof(String)) {
                var intVal = ((String)value).AsInt();
                if (intVal.HasValue)
                    return (T)(Object)intVal.Value;
                if (Enum.TryParse((String)value, true, out T tmpVal)) {
                    return tmpVal;
                }
            }
            throw ErrorHelper.Failed("Failed to convert");
        }
        public static TResult ConvertEnum<TSource, TResult>(this TSource source)
            where TSource : struct
            where TResult : struct {
            if (!typeof(TSource).IsEnum || !typeof(TResult).IsEnum)
                throw ErrorHelper.InvalidOperation("Operation must be carried out between enums only");
            return (TResult)(Object)(Int32)(Object)source;
        }
        public static Nullable<TResult> ConvertNullableEnum<TSource, TResult>(this Nullable<TSource> source)
            where TSource : struct
            where TResult : struct {
            if (!typeof(TSource).IsEnum || !typeof(TResult).IsEnum)
                throw ErrorHelper.InvalidOperation("Operation must be carried out between enums only");
            if (!source.HasValue)
                return null;
            return source.Value.ConvertEnum<TSource, TResult>();
        }

        public static String StripHtml(this String html) {
            if (html == null)
                return null;
            return Regex.Replace(html, @"<[^>]*>", String.Empty);
        }
        public static String Truncate(this String value, Int32 chars) {
            if (value == null)
                return null;
            if (value.Length <= chars)
                return value;
            return String.Format("{0}...", value.Substring(0, chars));
        }
        public static String Spaces(this Int32 count) {
            return "".PadLeft(count, ' ');
        }
        public static Boolean IsWhitespace(this Char @char) {
            return Char.IsWhiteSpace(@char);
        }
        public static T PropertyValueSafe<T>(this Object obj, String name) where T : class {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(obj, "obj");
            var info = obj
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (info == null)
                return null;
            return (T)info.GetValue(obj, null);
        }
        public static T PropertyValue<T>(this Object obj, String name) {
            Guard.NotNullOrEmpty(name, "name");
            Guard.NotNull(obj, "obj");
            var info = obj
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (info == null)
                throw ErrorHelper.NotFound("No such property");
            return (T)info.GetValue(obj, null);
        }
        public static void SafeDispose(this IDisposable disposable) {
            if (disposable != null)
                disposable.Dispose();
        }
        public static Byte[] ToBinary(this Exception ex) {
            var formatter = new BinaryFormatter();
            var ms = new MemoryStream();
            formatter.Serialize(ms, ex);
            return ms.Read();
        }
        public static Byte[] ToUtf8Bytes(this String value) {
            if (value == null)
                throw new NullReferenceException();
            return Encoding.UTF8.GetBytes(value);
        }
        public static Byte[] SerializeBinary(this Object value) {
            if (value == null)
                return null;
            var ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, value);
            return ms.Read();
        }
        public static T DeserializeBinary<T>(this Byte[] array) {
            Guard.NotNull(array, "array");
            return (T)new BinaryFormatter().Deserialize(new MemoryStream(array));
        }
        public static String HtmlEncode(this String value) {
            if (value == null)
                throw new NullReferenceException();
            var chars = new Dictionary<Char, String> {
                {'"', "&quot;"},
                {'&', "&amp;"},
                {'<', "&lt;"},
                {'>', "&gt;"},
                {'\'', "&#39;"}
            };
            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; i++) {
                if (chars.ContainsKey(value[i]))
                    sb.Append(chars[value[i]]);
                else
                    sb.Append(value[i]);
            }
            return sb.ToString();
        }
        public static String UrlEncode(this String value) {
            if (value == null)
                throw new NullReferenceException();
            return System.Uri.EscapeDataString(value);
        }
        /// <summary>
        /// Extension method to test whether the value is a base64 string
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <returns>Boolean value, true if the string is base64, otherwise false</returns>
        public static Boolean IsBase64String(this String value) {
            if (value == null || value.Length == 0 || value.Length % 4 != 0
                || value.Contains(' ') || value.Contains('\t') || value.Contains('\r') || value.Contains('\n'))
                return false;
            var index = value.Length - 1;
            if (value[index] == '=')
                index--;
            if (value[index] == '=')
                index--;
            for (var i = 0; i <= index; i++)
                if (IsInvalidBase64Char(value[i]))
                    return false;
            return true;
        }
        public static String ToCsvLine(this String[] data) {
            return data.Aggregate(new StringBuilder(), (sb, c) =>
                sb.AppendFormat("{0},", c.Return(x => x.Contains(',') ? "\"" + c.Replace("\"", "\"\"") + "\"" : c, ""))
            ).RemoveLast().ToString();
        }
        public static StringBuilder RemoveLast(this StringBuilder sb, Int32 count = 1) {
            if (sb == null)
                throw new NullReferenceException();
            if (count < 0 || count > sb.Length)
                throw ErrorHelper.ArgRange();
            return sb.Remove(sb.Length - count, count);
        }
        private static Boolean IsInvalidBase64Char(Char value) {
            var intValue = (Int32)value;
            if (intValue >= 48 && intValue <= 57)
                return false;
            if (intValue >= 65 && intValue <= 90)
                return false;
            if (intValue >= 97 && intValue <= 122)
                return false;
            return intValue != 43 && intValue != 47;
        }
    }
}