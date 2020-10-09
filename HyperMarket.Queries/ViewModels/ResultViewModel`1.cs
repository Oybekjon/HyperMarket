using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.ViewModels {
    public class ResultViewModel<T> : ResultViewModel {
        public ResultViewModel() : base(true) { }
        public ResultViewModel(T data) : this() {
            Data = data;
        }
        public T Data { get; set; }
    }
}
