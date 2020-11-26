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
            get => _DateCreated ??= DateTime.UtcNow;
            set => _DateCreated = value;
        }
    }
}
