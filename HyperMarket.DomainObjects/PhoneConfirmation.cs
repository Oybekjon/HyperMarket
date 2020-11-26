using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.DomainObjects
{
    public class PhoneConfirmation : DatedEntity
    {
        public long PhoneConfirmationId { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
        public int AttemptCount { get; set; }
        public bool Disabled { get; set; }
    }
}
