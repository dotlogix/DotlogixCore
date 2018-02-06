// ==================================================
// Copyright 2016(C) , DotLogix
// File:  NavService.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.UI.Navigation {
    public class NavService {
        private readonly List<NavPage> _navList;
        internal readonly NavFrame NavFrame;
        private int _currentPageIndex;

        public NavPage CurrentPage => _currentPageIndex < _navList.Count ? _navList[_currentPageIndex] : null;

        public bool CanGoForward => (_currentPageIndex + 1) < _navList.Count;
        public bool CanGoBack => (_currentPageIndex - 1) >= 0;


        public IEnumerable<NavPage> ForwardStack => GetForwardStack();
        public IEnumerable<NavPage> BackStack => GetBackStack();

        public NavService(NavFrame navFrame) {
            NavFrame = navFrame;
            _navList = new List<NavPage>();
        }

        public NavService(NavFrame navFrame, IEnumerable<NavPage> navPages) {
            NavFrame = navFrame;
            _navList = new List<NavPage>(navPages);
            _currentPageIndex = _navList.Count - 1;
            if(_currentPageIndex < 0)
                _currentPageIndex = 0;

            foreach(var navPage in _navList) {
                navPage.NavService = this;
            }
        }

        public void GoBack() {
            if(!CanGoBack)
                return;
            _currentPageIndex--;
            NavFrame.Page = CurrentPage;
        }

        public void GoForward() {
            if(!CanGoForward)
                return;
            _currentPageIndex++;
            NavFrame.Page = CurrentPage;
        }

        public void NavigateTo(NavPage page) {
            ClearForwardStack();
            page.NavService = this;
            _navList.Add(page);
            _currentPageIndex = _navList.Count - 1;
            NavFrame.Page = CurrentPage;
        }

        public void ClearForwardStack() {
            if(CanGoForward)
                _navList.RemoveRange(_currentPageIndex + 1, _navList.Count - _currentPageIndex - 1);
        }

        public void Clear() {
            _navList.Clear();
            _currentPageIndex = 0;
        }

        public void ClearBackStack() {
            if(CanGoBack)
                _navList.RemoveRange(0, _currentPageIndex - 1);
        }

        private IEnumerable<NavPage> GetForwardStack() {
            for(var i = _currentPageIndex + 1; i < _navList.Count; i++) {
                yield return _navList[i];
            }
        }

        private IEnumerable<NavPage> GetBackStack() {
            for(var i = _currentPageIndex - 1; i >= 0; i--) {
                yield return _navList[i];
            }
        }
    }
}
