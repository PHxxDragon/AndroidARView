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
            //webRequestHelper.GetModuleInformation(token, GetModuleInformationCallback, GetModuleInformationErrorCallback);
            webRequestHelper.GetInfoFromQRCode(token, GetModuleInformationCallback, GetModuleInformationErrorCallback);
        }

        private void GetModuleInformationErrorCallback(string obj)
        {
            Modal modal = Instantiate(modelPrefab, canvas);
            modal.SetModalContent("Error", "Invalid QR code");
            modal.DisableCancelButton();
            modal.OnConfirmButtonClick += GoBackToMenuScene;
            codeReader.StopScan();
            
        }

        private void GetModuleInformationCallback(ModuleARInformation obj)
        {
            if (obj.modelUrl == null)
            {
                Modal modal = Instantiate(modelPrefab, canvas);
                modal.SetModalContent("No Model", "This module has no model");
                modal.DisableCancelButton();
                modal.OnConfirmButtonClick += GoBackToMenuScene;
                codeReader.StopScan();
            } else {
                codeReader.StopScan();
                SceneChangeParam.moduleARInformation = obj;
                SceneManager.LoadScene("ARScene");
            }
        }

        private void GoBackToMenuScene()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}

