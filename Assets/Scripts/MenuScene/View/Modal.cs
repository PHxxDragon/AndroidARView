using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace EAR.View 
{
    public class Modal : MonoBehaviour
    {
        public event Action OnConfirmButtonClick;
        public event Action OnCancelButtonClick;

        [SerializeField]
        private TMP_Text titleText;
        [SerializeField]
        private TMP_Text contentText;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Button confirmButton;

        public void SetModalContent(string title, string message)
        {
            titleText.text = title;
            contentText.text = message;
        }

        public void DisableCancelButton(bool disable = true)
        {
            cancelButton.gameObject.SetActive(!disable);
        }

        void Start()
        {
            confirmButton.onClick.AddListener(ConfirmButtonClick);
            confirmButton.onClick.AddListener(CloseModal);
            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(CancelButtonClick);
                cancelButton.onClick.AddListener(CloseModal);
            }
        }

        private void ConfirmButtonClick()
        {
            OnConfirmButtonClick?.Invoke();
        }

        private void CancelButtonClick()
        {
            OnCancelButtonClick?.Invoke();
        }

        private void CloseModal()
        {
            Destroy(gameObject);
        }
    }
}

