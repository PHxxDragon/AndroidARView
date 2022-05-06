using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;

namespace EAR.View
{
    public class SelectModal : MonoBehaviour
    {
        public event Action<string> OnOptionSelected;

        [SerializeField]
        private Button panelButton;
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private GameObject optionContainer;
        [SerializeField]
        private SelectModalOption optionPrefab;

        void Awake()
        {
            panelButton.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
            cancelButton.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
        }

        public void PopulateData(string titleText, List<string> obj, List<string> objName)
        {
            if (obj.Count != objName.Count)
            {
                Debug.LogError("Object list and name list must have the same length");
                return;
            }

            title.text = titleText;

            foreach(Transform transform in optionContainer.transform)
            {
                Destroy(transform.gameObject);
            }

            for (int i = 0; i < obj.Count; i++)
            {
                SelectModalOption selectModalOption = Instantiate(optionPrefab, optionContainer.transform);
                selectModalOption.PopulateData(obj[i], objName[i]);
                selectModalOption.OnOptionClick += (obj) =>
                {
                    OnOptionSelected?.Invoke(obj);
                    Destroy(gameObject);
                };
            }
        }
    }
}

