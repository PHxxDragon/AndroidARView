using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EAR.View
{
    public abstract class ListView<T1, T2> : ViewInterface 
        where T1: ListItemView<T2> 
    {
        public const int LIMIT = 10;

        [SerializeField]
        protected GameObject viewPrefab;
        [SerializeField]
        protected GameObject container;
        [SerializeField]
        protected TMP_Dropdown pageDropdown;

        protected virtual void Awake()
        {
            pageDropdown.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });
        }

        public virtual void PopulateData(List<T2> data, int pageCount)
        {
            if (pageCount != 0)
            {
                pageDropdown.ClearOptions();
                List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
                for (int i = 0; i < pageCount; i++)
                {
                    TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                    optionData.text = (i + 1).ToString();
                    optionDatas.Add(optionData);
                }
                pageDropdown.AddOptions(optionDatas);
            }

            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (T2 datum in data)
            {
                T1 modelView = Instantiate(viewPrefab, container.transform).GetComponent<T1>();
                modelView.PopulateData(datum);
            }
        }
    }
}

