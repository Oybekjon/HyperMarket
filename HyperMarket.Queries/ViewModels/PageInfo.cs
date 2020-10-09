using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperMarket.Queries.ViewModels {
    public class PageInfo {
        private Boolean _NextButton;
        private Boolean _PreviousButton;
        private List<Int32> _Pages;
        private Int32 _CurrentPage;
        private Int32 _TotalPages;
        public Boolean NextButton {
            get { return _NextButton; }
        }
        public Boolean PreviousButton {
            get { return _PreviousButton; }
        }
        public List<Int32> Pages {
            get { return _Pages; }
        }
        public Int32 CurrentPage {
            get { return _CurrentPage; }
        }
        public Int32 TotalPages {
            get { return _TotalPages; }
        }
        public PageInfo(Int32 currentPage, Int32 totalPages) {
            if (totalPages == 0)
                totalPages = 1;
            _CurrentPage = currentPage;
            _TotalPages = totalPages;
            var fromCount = currentPage - (currentPage % 10) + 1;
            var toCount = Math.Min(fromCount + 10, totalPages);
            _Pages = Enumerable.Range(fromCount, toCount).ToList();
            _PreviousButton = currentPage > 1;
            _NextButton = currentPage < totalPages;
        }
    }
}