using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperMarket.Queries.ViewModels {
    public class PageInfo {
        private bool _NextButton;
        private bool _PreviousButton;
        private List<int> _Pages;
        private int _CurrentPage;
        private int _TotalPages;
        public bool NextButton {
            get { return _NextButton; }
        }
        public bool PreviousButton {
            get { return _PreviousButton; }
        }
        public List<int> Pages {
            get { return _Pages; }
        }
        public int CurrentPage {
            get { return _CurrentPage; }
        }
        public int TotalPages {
            get { return _TotalPages; }
        }
        public PageInfo(int currentPage, int totalPages) {
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