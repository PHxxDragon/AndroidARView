using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace EAR.View
{
    public class ModuleListView : ViewInterface
    {
        public event Action<int> ModuleListRefreshEvent;
        public event Action BackButtonClickEvent;

        [SerializeField]
        private GameObject compositePrefab;
        [SerializeField]
        private GameObject container;

        [SerializeField]
        private Button backButton;

        private int courseId;

        void Awake()
        {
            backButton.onClick.AddListener(() =>
            {
                BackButtonClickEvent?.Invoke();
            });
        }

        public void PopulateData(List<object> moduleDatas)
        {
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
                courseId = (int)args;
            }

            ModuleListRefreshEvent?.Invoke(courseId);
        }
    }
}
