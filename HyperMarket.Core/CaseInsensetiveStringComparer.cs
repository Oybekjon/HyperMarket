using System;
using System.Collections.Generic;
namespace HyperMarket {
    public class CaseInsensetiveStringComparer : IEqualityComparer<String> {
        public Boolean Equals(String x, String y) {
            return String.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }
        public Int32 GetHashCode(String obj) {
            if (obj != null)
                return obj.ToLower().GetHashCode();
            return 0;
        }
    }
}