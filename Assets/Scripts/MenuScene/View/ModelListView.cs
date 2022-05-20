using TMPro;
using UnityEngine;
using System;
using EAR.SceneChange;

namespace EAR.View
{
    public class ModelListView : ListView<ModelView, ModelDataObject>
    {
        public enum ModelType
        {
            All, Uploaded, Bought
        }

        public event Action<int, int, ModelType, string> ModelListRefreshEvent;
        public event Action OnGoBack;

        [SerializeField]
        private TMP_Dropdown modelTypeDropdown;
        [SerializeField]
        private SearchBar searchBar;

        protected override void Awake()
        {
            base.Awake();
            modelTypeDropdown.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });
            searchBar.OnSearch += (text) =>
            {
                Refresh();
            };
            LoadState();
        }

        public override void Refresh(object args = null)
        {
            base.Refresh(args);
            SaveState();
            ModelListRefreshEvent?.Invoke(MenuSceneParam.modelPage, LIMIT, MenuSceneParam.modelType, MenuSceneParam.modelKeyword);
        }

        public override void GoBack()
        {
            OnGoBack?.Invoke();
        }

        private void SaveState()
        {
            MenuSceneParam.modelPage = pageDropdown.value + 1;
            MenuSceneParam.modelKeyword = searchBar.GetText();
            MenuSceneParam.modelType = (ModelType) modelTypeDropdown.value;
        }

        private void LoadState()
        {
            pageDropdown.value = MenuSceneParam.modelPage - 1;
            searchBar.SetText(MenuSceneParam.modelKeyword);
            modelTypeDropdown.value = (int) MenuSceneParam.modelType;
        }
    }
}
