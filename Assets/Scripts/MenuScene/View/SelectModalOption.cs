using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace EAR.View
{
    public class SelectModalOption : MonoBehaviour
    {
        public event Action<string> OnOptionClick;

        [SerializeField]
        private TMP_Text optionName;
        [SerializeField]
        private Button button;

        private string obj;

        void Awake()
        {
            button.onClick.AddListener(() =>
            {
                OnOptionClick?.Invoke(obj);
            });
        }

        public void PopulateData(string obj, string name)
        {
            this.obj = obj;
            optionName.text = name;
        }
    }

}
