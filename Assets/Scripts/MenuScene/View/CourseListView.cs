using UnityEngine;
using System;
using TMPro;
using EAR.SceneChange;

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
                SaveState();
                Refresh();
            });
            searchBar.OnSearch += (text) =>
            {
                SaveState();
                Refresh();
            };
            LoadState();
        }

        public override void Refresh(object args = null)
        {
            base.Refresh(args);
            SaveState();
            CourseListRefreshEvent?.Invoke(MenuSceneParam.coursePage, LIMIT, MenuSceneParam.courseType, MenuSceneParam.courseKeyword);
        }

        public override void GoBack()
        {
            Application.Quit();
        }

        private void SaveState()
        {
            MenuSceneParam.coursePage = pageDropdown.value + 1;
            MenuSceneParam.courseKeyword = searchBar.GetText();
            MenuSceneParam.courseType = (CourseType)courseTypeDropdown.value;
        }

        private void LoadState()
        {
            pageDropdown.value = MenuSceneParam.coursePage - 1;
            searchBar.SetText(MenuSceneParam.courseKeyword);
            courseTypeDropdown.value = (int)MenuSceneParam.courseType;
        }
    }
}

