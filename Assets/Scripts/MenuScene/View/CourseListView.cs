using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EAR.View
{
    [RequireComponent(typeof(Animator))]
    public class CourseListView : ViewInterface
    {
        public event Action<int> CourseListRefreshEvent;

        [SerializeField]
        private GameObject coursePrefab;

        [SerializeField]
        private GameObject container;

        private Animator animator;
        private string transparentClose = "TransparentClosing";
        private string transparentOpen = "TransparentOpening";

        private int workspaceId;
        private Dictionary<int, CourseView> courseViews = new Dictionary<int, CourseView>();

        public void PopulateData(List<CourseData> courseDatas)
        {
            foreach(Transform transform in container.transform)
            {
                Destroy(transform.gameObject);
            }
            courseViews.Clear();
            foreach (CourseData data in courseDatas)
            {
                CourseView courseView = Instantiate(coursePrefab, container.transform).GetComponent<CourseView>();
                courseView.PopulateData(data);
                courseViews.Add(data.id, courseView);
            }
        }

        public void PopulateData(Sprite sprite, int id)
        {
            courseViews[id].PopulateData(sprite);
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
                workspaceId = (int)args;
            }
        }

        public override void Refresh(object args = null)
        {
            if (args != null)
            {
                workspaceId = (int)args;
            }

            CourseListRefreshEvent?.Invoke(workspaceId);
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

