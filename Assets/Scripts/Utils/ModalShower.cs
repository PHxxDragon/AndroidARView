using UnityEngine;
using EAR.View;
using System;

namespace EAR
{
    public class ModalShower : MonoBehaviour
    {
        private const string ERROR = "Error";

        [SerializeField]
        private Modal modalPrefab;

        [SerializeField]
        private RectTransform canvas;

        public void ShowErrorModal(string error, Action confirm = null)
        {
            Modal modal = Instantiate(modalPrefab, canvas);
            modal.SetModalContent(LocalizationUtils.GetLocalizedText(ERROR), error);
            modal.DisableCancelButton();
            modal.OnConfirmButtonClick += () =>
            {
                confirm?.Invoke();
                Destroy(modal.gameObject);
            };
        }
    }

}
