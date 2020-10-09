using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperMarket {
    public interface IMultipartParameter {
        String ParamName { get; }
        Byte[] Value { get; }
        String FileName { get; }
        String ContentType { get; }
    }
}
