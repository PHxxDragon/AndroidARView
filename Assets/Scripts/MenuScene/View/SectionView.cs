using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace EAR.View
{
    public class SectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text sectionTitle;
        [SerializeField]
        private Button button;

        private UnityAction listener;

        public void PopulateData(SectionData data)
        {
            sectionTitle.text = data.name;
            if (listener != null)
            {
                button.onClick.RemoveListener(listener);
            }
            listener = () =>
            {
                data.sectionClickEvent?.Invoke();
            };
            button.onClick.AddListener(listener);
        }
}
}

