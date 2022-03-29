using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EAR.View
{
    public class ModelListView : ListView<ModelView, ModelDataObject>
    {
        public enum ModelType
        {
            Uploaded, Bought
        }

        public event Action<int, int, ModelType> ModelListRefreshEvent;

        [SerializeField]
        private TMP_Dropdown modelTypeDropdown;

        protected override void Awake()
        {
            base.Awake();
            modelTypeDropdown.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });
        }

        public override void Refresh(object args = null)
        {
            ModelListRefreshEvent?.Invoke(pageDropdown.value + 1, LIMIT, (ModelType) modelTypeDropdown.value);
        }
    }
}
