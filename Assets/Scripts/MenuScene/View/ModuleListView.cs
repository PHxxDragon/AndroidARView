using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EAR.View
{
    [RequireComponent(typeof(Animator))]
    public class ModuleListView : ViewInterface
    {
        public event Action<int> ModuleListRefreshEvent;

        [SerializeField]
        private GameObject modulePrefab;
        [SerializeField]
        private GameObject container;

        private Animator animator;
        private string transparentClose = "TransparentClosing";
        private string transparentOpen = "TransparentOpening";

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

        public override void CloseView()
        {
            animator.enabled = true;
            animator.Play(transparentClose);
        }

        public override void OpenView(object args = null)
        {
            animator.enabled = true;
            animator.Play(transparentOpen);
            if (args != null)
            {
                courseId = (int)args;
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

        public void DisableAnimator()
        {
            animator.enabled = false;
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
    }
}
