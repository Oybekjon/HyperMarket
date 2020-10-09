using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HyperMarket {
    public class PostFileInfo : IMultipartParameter {
        public String ContentType { get; private set; }
        public String FileName { get; private set; }
        public Byte[] Data { get; private set; }
        public String ParamName { get; private set; }
        public PostFileInfo(Stream data, String fileName, String paramName, String contentType) {
            Guard.NotNull(data, "data");
            Guard.NotNullOrEmpty(fileName, "fileName");
            Guard.NotNullOrEmpty(paramName, "paramName");
            if (!(data.Position == 0 || data.CanSeek) || !data.CanRead)
                throw ErrorHelper.InvalidArgument("Invalid stream. It must be readable, at the position 0 or seekable.");
            Data = data.Read();
            FileName = fileName;
            ParamName = paramName;
            if (String.IsNullOrWhiteSpace(contentType))
                DetectContentType();
            else
                ContentType = contentType;
        }
        public PostFileInfo(Stream data, String fileName, String paramName) : this(data, fileName, paramName, null) { }
        public PostFileInfo(Byte[] data, String fileName, String paramName, String contentType) {
            Guard.NotNull(data, "data");
            Guard.NotNullOrEmpty(fileName, "fileName");
            Guard.NotNullOrEmpty(paramName, "paramName");
            Data = data;
            FileName = fileName;
            ParamName = paramName;
            if (String.IsNullOrWhiteSpace(contentType))
                DetectContentType();
            else
                ContentType = contentType;
        }
        public PostFileInfo(Byte[] data, String fileName, String paramName) : this(data, fileName, paramName, null) { }
        private void DetectContentType() {
            if (!String.IsNullOrWhiteSpace(FileName)) {
                var helper = new FileSystemHelper();
                ContentType = helper.GetContentType(FileName);
            }
        }
        Byte[] IMultipartParameter.Value {
            get { return Data; }
        }
    }
}