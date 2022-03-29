using UnityEngine;
using EAR.View;

namespace EAR
{
    public class ModalShower : MonoBehaviour
    {
        private const string ERROR = "Error";

        [SerializeField]
        private Modal modalPrefab;

        [SerializeField]
        private RectTransform canvas;

        public void ShowErrorModal(string error)
        {
            Modal modal = Instantiate(modalPrefab, canvas);
            modal.SetModalContent(Utils.GetLocalizedText(ERROR), error);
            modal.DisableCancelButton();
            modal.OnConfirmButtonClick += () =>
            {
                Destroy(modal.gameObject);
            };
        }
    }

}
