using UnityEngine;
using UnityEngine.UI;

namespace EAR.View
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleExtension : MonoBehaviour
    {
        public GameObject activeWhenFalse;
        public GameObject activeWhenTrue;
        public bool findToggleGroupInParent = true;

        private Toggle toggle;

        void Awake()
        {
            toggle = GetComponent<Toggle>();
            if (findToggleGroupInParent)
            {
                toggle.group = GetComponentInParent<ToggleGroup>();
            }
        }

        void Start()
        {
            OnSetGameObjectActive(toggle.isOn);
            toggle.onValueChanged.AddListener(OnSetGameObjectActive);
        }

        private void OnSetGameObjectActive(bool isToggleOn)
        {
            if (activeWhenFalse != null)
            {
                activeWhenFalse.SetActive(!isToggleOn);
            }
            if (activeWhenTrue != null)
            {
                activeWhenTrue.SetActive(isToggleOn);    
            }
        }
    }
}

