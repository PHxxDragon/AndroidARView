using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
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
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private GameObject left;
        [SerializeField]
        private GameObject right;

        public void PopulateData(List<SectionData> sectionStack)
        {
            foreach (Transform transform in container.transform)
            {
                Destroy(transform.gameObject);
            }

            /*            if (sectionStack.Count > 3)
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
                        }*/

            for (int i = 0; i < sectionStack.Count; i++)
            {
                BreadCrumbItemView breadCrumbItemView = Instantiate(breadcrumbItemPrefab, container.transform);
                breadCrumbItemView.PopulateData(sectionStack[i]);
                int j = i;
                breadCrumbItemView.OnClick += () =>
                {
                    OnBreadCrumbItemClick?.Invoke(j);
                };
            }

            StartCoroutine(ScrollToLast());
            scrollRect.onValueChanged.AddListener((value) => {
                left.gameObject.SetActive(value.x > 0.05);
                right.gameObject.SetActive(value.x < 0.95);
            });
        }

        private IEnumerator ScrollToLast()
        {
            yield return new WaitForEndOfFrame();
            scrollRect.horizontalNormalizedPosition = 1;
        }
    }
}

