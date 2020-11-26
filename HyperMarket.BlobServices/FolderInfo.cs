using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.BlobServices
{
    public class FolderInfo : EntryInfo
    {
        private List<EntryInfo> _Children;
        public List<EntryInfo> Children
        {
            get => _Children ??= new List<EntryInfo>();
            set => _Children = value;
        }

        public override bool IsFolder => true;
    }
}
