using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class BreadCrumbItemView : MonoBehaviour
    {
        public event Action OnClick;

        [SerializeField]
        private Button button;
        [SerializeField]
        private TMP_Text text;

        void Awake()
        {
            button.onClick.AddListener(Click);
        }

        private void Click()
        {
            OnClick?.Invoke();
        }

        void OnDestroy()
        {
            if (button)
            {
                button.onClick.RemoveListener(Click);
            }
            OnClick = null;
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void PopulateData(SectionData sectionData)
        {
            text.text = sectionData.name;
        }
    }
}

