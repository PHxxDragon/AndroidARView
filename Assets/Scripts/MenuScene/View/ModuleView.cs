using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class ModuleView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;

        [SerializeField]
        private Button button;

        private int id;
        private Action<int> moduleClickEvent;

        public void PopulateData(ModuleData data)
        {
            id = data.id;
            title.text = data.name;
            moduleClickEvent = data.moduleClickEvent;
        }

        void Start()
        {
            button.onClick.AddListener(ButtonClickEventSubscriber);
        }

        private void ButtonClickEventSubscriber()
        {
            moduleClickEvent?.Invoke(id);
        }
    }
}

