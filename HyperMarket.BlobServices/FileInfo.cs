using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices
{
    public class FileInfo : EntryInfo
    {
        public override bool IsFolder => false;
        public long Size { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
