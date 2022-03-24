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
            modal.SetModalContent(Utils.GetLocalizedText("Error"), Utils.GetLocalizedText("InvalidQRCode"));
            modal.DisableCancelButton();
            modal.OnConfirmButtonClick += GoBackToMenuScene;
            codeReader.StopScan();
            
        }

        private void GetModuleInformationCallback(ModuleARInformation obj)
        {
            if (obj.modelUrl == null)
            {
                Modal modal = Instantiate(modelPrefab, canvas);
                modal.SetModalContent(Utils.GetLocalizedText("NoModel"), Utils.GetLocalizedText("NoModelMessage"));
                modal.DisableCancelButton();
                modal.OnConfirmButtonClick += GoBackToMenuScene;
                codeReader.StopScan();
            } else {
                codeReader.StopScan();
                ARSceneParam.moduleARInformation = obj;
                SceneManager.LoadScene("ARScene");
            }
        }

        private void GoBackToMenuScene()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}

