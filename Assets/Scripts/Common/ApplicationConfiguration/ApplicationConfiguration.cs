using System;
using UnityEngine;

namespace EAR
{
    [CreateAssetMenu(menuName = "EAR/Configuration")]
    public class ApplicationConfiguration : ScriptableObject
    {
        [SerializeField]
        private string serverName;

        [SerializeField]
        private string loginPath;

        [SerializeField]
        private string profilePath;

/*        [SerializeField]
        private string armodulePath;*/

        [SerializeField]
        private string modelPath;

        [SerializeField]
        private string boughtModelListPath;

        [SerializeField]
        private string uploadedModelListPath;

        [SerializeField]
        private string qrCodePath;

        [SerializeField]
        private string categoriesPath;

/*        [SerializeField]
        private string armoduleQRPath;*/

        public string GetServerName()
        {
            return serverName;
        }

        public string GetLoginPath()
        {
            return serverName + loginPath;
        }

        public string GetProfilePath()
        {
            return serverName + profilePath;
        }

        public string GetWorkspacePath()
        {
            return "";
        }

/*        public string GetARModulePath(int moduleId)
        {
            return serverName + armodulePath + "/" + moduleId;
        }*/

        public string GetBoughtModelListPath(int page, int limit)
        {
            return serverName + boughtModelListPath + "?page=" + page + "&limit=" + limit;
        }

        public string GetUploadedModelListPath(int page, int limit)
        {
            return serverName + uploadedModelListPath + "?page=" + page + "&limit=" + limit;
        }

        public string GetModelPath(int modelId)
        {
            return serverName + modelPath + "/" + modelId;
        }

        public string GetModelARDataPath(int modelId)
        {
            return serverName + modelPath + "/" + modelId + "/get-model-ar-data";
        }

        public string GetQRCodePath(string qrCode)
        {
            return serverName + qrCodePath + "/" + qrCode;
        }

        public string GetCategoryPath(string langCode)
        {
            return serverName + categoriesPath + "?langCode=" + langCode;
        }

/*        public string GetARModuleQRPath(string qrToken)
        {
            return serverName + armoduleQRPath + "/" + qrToken;
        }*/
    }
}

