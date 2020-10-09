using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.ViewModels {
    public interface IListViewModel {
        IList Data { get; }
        PageInfo PageInfo { get; set; }
    }
}
