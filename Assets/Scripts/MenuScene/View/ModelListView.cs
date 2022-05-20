using TMPro;
using UnityEngine;
using System;

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
        }

        public override void Refresh(object args = null)
        {
            base.Refresh(args);
            ModelListRefreshEvent?.Invoke(pageDropdown.value + 1, LIMIT, (ModelType)modelTypeDropdown.value, searchBar.GetText());
        }

        public override void GoBack()
        {
            OnGoBack?.Invoke();
        }
    }
}
