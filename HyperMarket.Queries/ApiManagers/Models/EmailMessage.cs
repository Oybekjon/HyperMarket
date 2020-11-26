using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HyperMarket.Queries.ApiManagers.Models
{
    public class EmailMessage
    {
        public string[] Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Dictionary<string, Stream> Attachments { get; set; }
    }
}
