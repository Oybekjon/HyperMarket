using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperMarket.Queries.ViewModels {
    public class ListViewModel<T> : ResultViewModel, IListViewModel {
        public ListViewModel():base(true) { }
        public ListViewModel(List<T> data) : base(true) {
            Data = data;
        }
        public ListViewModel(List<T> data, Int32 currentPage, Int32 totalPages) : base(true) {
            Data = data;
            PageInfo = new PageInfo(currentPage, totalPages);
        }
        public ListViewModel(IEnumerable<T> data, Int32 currentPage, Int32 totalPages) : base(true) {
            if (data != null)
                Data = new List<T>(data);
            PageInfo = new PageInfo(currentPage, totalPages);
        }
        public List<T> Data { get; set; }
        public PageInfo PageInfo { get; set; }
        IList IListViewModel.Data {
            get { return Data; }
        }
    }
}
