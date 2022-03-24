using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EAR.View
{
    public class ModelListView : ViewInterface
    {
        public enum ModelType
        {
            Uploaded, Bought
        }

        public event Action<int, int, ModelType> ModelListRefreshEvent;
        public const int MODEL_LIMIT = 10;

        [SerializeField]
        private GameObject modelPrefab;
        [SerializeField]
        private GameObject container;
        [SerializeField]
        private TMP_Dropdown modelTypeDropdown;
        [SerializeField]
        private TMP_Dropdown modelPageDropdown;

        void Awake()
        {
            modelTypeDropdown.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });
            modelPageDropdown.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });
        }

        public void PopulateData(List<ModelDataObject> modelDatas, int pageCount)
        {
            if (pageCount != 0)
            {
                modelPageDropdown.ClearOptions();
                List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
                for (int i = 0; i < pageCount; i++)
                {
                    TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                    optionData.text = (i + 1).ToString();
                    optionDatas.Add(optionData);
                }
                modelPageDropdown.AddOptions(optionDatas);
            }

            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (ModelDataObject data in modelDatas)
            {
                ModelView modelView = Instantiate(modelPrefab, container.transform).GetComponent<ModelView>();
                modelView.PopulateData(data);
            }
        }

        public override void Refresh(object args = null)
        {
            ModelListRefreshEvent?.Invoke(modelPageDropdown.value + 1, MODEL_LIMIT, (ModelType) modelTypeDropdown.value);
        }
    }
}
