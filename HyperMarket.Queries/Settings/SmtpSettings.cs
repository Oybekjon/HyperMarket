using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Settings
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseTls { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
