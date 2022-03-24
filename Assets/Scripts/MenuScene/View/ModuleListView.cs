using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EAR.View
{
    public class ModuleListView : ViewInterface
    {
        public event Action<int> ModuleListRefreshEvent;

        [SerializeField]
        private GameObject modulePrefab;
        [SerializeField]
        private GameObject container;

        private int courseId;

        public void PopulateData(List<ModuleData> moduleDatas)
        {
            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }
            foreach(ModuleData data in moduleDatas)
            {
                ModuleView moduleView = Instantiate(modulePrefab, container.transform).GetComponent<ModuleView>();
                moduleView.PopulateData(data);
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
