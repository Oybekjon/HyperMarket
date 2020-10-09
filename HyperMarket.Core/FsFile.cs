﻿using System;
using System.IO;
namespace HyperMarket {
    public class FsFile {
        protected String FullFileName;
        public virtual String Text {
            get { return File.ReadAllText(FullFileName); }
        }
        public virtual Stream InMemoryStream {
            get { return new MemoryStream(File.ReadAllBytes(FullFileName)); }
        }
        public virtual Byte[] Bytes {
            get { return File.ReadAllBytes(FullFileName); }
        }
        public virtual String FullFilePath {
            get { return FullFileName; }
        }
        public FsFile(String fileName) {
            Guard.NotNull(fileName, "fileName");
            FullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(FullFileName))
                throw ErrorHelper.NotFound(String.Format("File \"{0}\" not found", FullFileName));
        }
        protected FsFile() { }
    }
}