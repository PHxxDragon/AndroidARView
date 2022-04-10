using UnityEngine;
using EAR.QRCode;
using EAR.SceneChange;
using EAR.WebRequest;
using UnityEngine.SceneManagement;
using EAR.View;

namespace EAR.Editor.Presenter
{
    public class ChangeScenePresenter : MonoBehaviour
    {
        [SerializeField]
        QRCodeReader codeReader;
        [SerializeField]
        WebRequestHelper webRequestHelper;
        [SerializeField]
        Modal modelPrefab;
        [SerializeField]
        Transform canvas;
        

        void Start()
        {
            codeReader.QRCodeRecognizedEvent += QRCodeRecognizedEventSubscriber;
        }

        private void QRCodeRecognizedEventSubscriber(string token)
        {
            Debug.Log(token);
            webRequestHelper.GetInfoFromQRCode(token, GetModuleInformationCallback, GetModuleInformationErrorCallback);
        }

        private void GetModuleInformationErrorCallback(string obj)
        {
            Debug.Log("obj: " + obj);
            Modal modal = Instantiate(modelPrefab, canvas);
            modal.SetModalContent(LocalizationUtils.GetLocalizedText("Error"), LocalizationUtils.GetLocalizedText("InvalidQRCode"));
            modal.DisableCancelButton();
            modal.OnConfirmButtonClick += GoBackToMenuScene;
            codeReader.StopScan();
            
        }

        private void GetModuleInformationCallback(AssetInformation assetInformation)
        {
            codeReader.StopScan();
            ARSceneParam.assetInformation = assetInformation;
            SceneManager.LoadScene("ARScene");
        }

        private void GoBackToMenuScene()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}

