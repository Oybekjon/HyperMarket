using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.DomainObjects
{
    public class DatedEntity
    {
        private DateTime? _DateCreated;
        public DateTime DateCreated
        {
            get => (_DateCreated = _DateCreated ?? DateTime.UtcNow).Value;
            set => _DateCreated = value;
        }
    }
}
