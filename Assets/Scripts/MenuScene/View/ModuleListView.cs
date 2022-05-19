using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace EAR.View
{
    public class ModuleListView : ViewInterface
    {
        public event Action<int, string> ModuleListRefreshEvent;
        public event Action BackButtonClickEvent;

        [SerializeField]
        private TMP_Text headerTitle;
        [SerializeField]
        private GameObject compositePrefab;
        [SerializeField]
        private GameObject container;
        [SerializeField]
        private GameObject loadingIcon;
        [SerializeField]
        private GameObject empty;

        [SerializeField]
        private Button backButton;



        private (int, string) courseId;

        void Awake()
        {
            loadingIcon.gameObject.SetActive(false);
            empty.gameObject.SetActive(false);
            backButton.onClick.AddListener(() =>
            {
                BackButtonClickEvent?.Invoke();
            });
        }

        public void SetHeaderTitle(string text)
        {
            headerTitle.text = text;
        }

        public void PopulateData(List<object> moduleDatas)
        {
            container.gameObject.SetActive(true);
            loadingIcon.gameObject.SetActive(false);
            empty.gameObject.SetActive(moduleDatas.Count == 0);            

            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }
            foreach(object data in moduleDatas)
            {
                CompositeView compositeView = Instantiate(compositePrefab, container.transform).GetComponent<CompositeView>();
                compositeView.PopulateData(data);
            }
        }

        public override void Refresh(object args = null)
        {
            if (args != null)
            {
                courseId = ((int, string)) args;
            }

            loadingIcon.gameObject.SetActive(true);
            container.gameObject.SetActive(false);
            empty.gameObject.SetActive(false);
            ModuleListRefreshEvent?.Invoke(courseId.Item1, courseId.Item2);
        }
    }
}
