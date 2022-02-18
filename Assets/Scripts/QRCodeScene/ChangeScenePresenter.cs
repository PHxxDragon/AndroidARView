using UnityEngine;
using EAR.QRCode;
using EAR.SceneChange;
using EAR.WebRequest;
using UnityEngine.SceneManagement;

namespace EAR.Editor.Presenter
{
    public class ChangeScenePresenter : MonoBehaviour
    {
        [SerializeField]
        QRCodeReader codeReader;
        [SerializeField]
        WebRequestHelper webRequestHelper;

        void Start()
        {
            codeReader.QRCodeRecognizedEvent += QRCodeRecognizedEventSubscriber;
        }

        private void QRCodeRecognizedEventSubscriber(string token)
        {
            webRequestHelper.GetModuleInformation(token, GetModuleInformationCallback, GetModuleInformationErrorCallback);
        }

        private void GetModuleInformationErrorCallback(string obj)
        {
            Debug.LogError(obj);
        }

        private void GetModuleInformationCallback(ModuleARInformation obj)
        {
            if (obj.modelUrl == null)
            {
                Debug.LogError("Model not choosen for module yet");
            } else {
                codeReader.StopScan();
                SceneChangeParam.moduleARInformation = obj;
                SceneManager.LoadScene("ARScene");
            }
        }
    }
}

