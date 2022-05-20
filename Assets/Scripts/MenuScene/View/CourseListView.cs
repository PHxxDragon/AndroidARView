using UnityEngine;
using System;
using TMPro;

namespace EAR.View
{
    public class CourseListView : ListView<CourseView, CourseData>
    {
        public enum CourseType
        {
            All, Owned, Joined
        }

        public event Action<int, int, CourseType, string> CourseListRefreshEvent;

        [SerializeField]
        private TMP_Dropdown courseTypeDropdown;
        [SerializeField]
        private SearchBar searchBar;

        protected override void Awake()
        {
            base.Awake();
            courseTypeDropdown.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });
            searchBar.OnSearch += (text) =>
            {
                Refresh();
            };
        }

        public override void Refresh(object args = null)
        {
            base.Refresh(args);
            CourseListRefreshEvent?.Invoke(pageDropdown.value + 1, LIMIT, (CourseType) courseTypeDropdown.value, searchBar.GetText());
        }

        public override void GoBack()
        {
            Application.Quit();
        }
    }
}

