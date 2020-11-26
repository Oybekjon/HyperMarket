using System.Collections;
using System.Collections.Generic;

namespace HyperMarket.Queries.ViewModels
{
    public class ListViewModel<T> : ResultViewModel, IListViewModel
    {
        public List<T> Data { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public int TotalRecords { get; set; }

        public ListViewModel() : base(true) { }

        public ListViewModel(List<T> data) : base(true)
        {
            Data = data;
        }

        public ListViewModel(List<T> data, int totalRecords) : base(true)
        {
            Data = data;
            TotalRecords = totalRecords;
        }

        public ListViewModel(IEnumerable<T> data, int totalRecords) : base(true)
        {
            if (data != null)
                Data = new List<T>(data);
            TotalRecords = totalRecords;
        }

        public ListViewModel(List<T> data, int totalRecords, int limit, int offset) : base(true)
        {
            Data = new List<T>(data);
            TotalRecords = totalRecords;
            Limit = limit;
            Offset = offset;
        }

        IList IListViewModel.Data
        {
            get { return Data; }
        }
    }
}
