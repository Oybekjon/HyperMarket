using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices
{
    public abstract class EntryInfo
    {
        public abstract bool IsFolder { get; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public EntryInfo Parent { get; set; }
    }
}
