using UnityEngine;
using System.Collections.Generic;
using System;

namespace EAR.View
{
    public class BreadCrumbView : MonoBehaviour
    {
        public event Action<int> OnBreadCrumbItemClick;

        [SerializeField]
        private GameObject container;
        [SerializeField]
        private BreadCrumbItemView breadcrumbItemPrefab;

        public void PopulateData(List<SectionData> sectionStack)
        {
            foreach (Transform transform in container.transform)
            {
                Destroy(transform.gameObject);
            }

            if (sectionStack.Count > 3)
            {
                BreadCrumbItemView breadCrumbItemView = Instantiate(breadcrumbItemPrefab, container.transform);
                breadCrumbItemView.SetText("...");
                RectTransform rectTransform = breadCrumbItemView.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x / 2, rectTransform.sizeDelta.y);
                breadCrumbItemView.OnClick += () =>
                {
                    OnBreadCrumbItemClick?.Invoke(sectionStack.Count - 2);
                };
            }

            for (int i = Math.Max(sectionStack.Count - 3, 0); i < sectionStack.Count; i++)
            {
                BreadCrumbItemView breadCrumbItemView = Instantiate(breadcrumbItemPrefab, container.transform);
                breadCrumbItemView.PopulateData(sectionStack[i]);
                int j = i;
                breadCrumbItemView.OnClick += () =>
                {
                    OnBreadCrumbItemClick?.Invoke(j);
                };
            }
        }
    }
}

