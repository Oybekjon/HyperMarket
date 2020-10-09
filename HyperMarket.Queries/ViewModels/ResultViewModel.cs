using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.ViewModels {
    public class ResultViewModel {
        public Boolean Success { get; private set; }
        public ResultViewModel(Boolean success) {
            Success = success;
        }
    }
}
