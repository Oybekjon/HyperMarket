using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.BlobServices.Amazon
{
    public class AmazonFolderInfo : FolderInfo
    {
        public S3Region Location { get; set; }
    }
}
