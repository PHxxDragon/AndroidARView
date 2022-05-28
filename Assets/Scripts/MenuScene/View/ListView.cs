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
        [SerializeField]
        protected GameObject emptyIndicator;
        [SerializeField]
        protected GameObject loadingCircle;

        protected virtual void Awake()
        {
            emptyIndicator.gameObject.SetActive(false);
            loadingCircle.gameObject.SetActive(false);
            pageDropdown.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });
        }

        public virtual void KeepData()
        {
            loadingCircle.gameObject.SetActive(false);
            container.gameObject.SetActive(true);
        }

        public virtual void PopulateData(List<T2> data, int pageCount)
        {
            loadingCircle.gameObject.SetActive(false);
            container.gameObject.SetActive(true);

            if (data.Count != 0)
            {
                emptyIndicator.gameObject.SetActive(false);
            } else
            {
                emptyIndicator.gameObject.SetActive(true);
            }

            if (pageCount != 0)
            {
                int oldPage = pageDropdown.value;
                pageDropdown.ClearOptions();
                List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
                for (int i = 0; i < pageCount; i++)
                {
                    TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                    optionData.text = (i + 1).ToString();
                    optionDatas.Add(optionData);
                }
                pageDropdown.AddOptions(optionDatas);
                pageDropdown.value = oldPage;
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

        public override void Refresh(object args = null)
        {
            loadingCircle.gameObject.SetActive(true);
            container.gameObject.SetActive(false);
            emptyIndicator.gameObject.SetActive(false);
        }
    }
}

