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

        [SerializeField]
        private string armodulePath;

        [SerializeField]
        private string modelPath;

        [SerializeField]
        private string armoduleQRPath;

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

        public string GetARModulePath(int moduleId)
        {
            return serverName + armodulePath + "/" + moduleId;
        }

        public string GetModelPath(int modelId)
        {
            return serverName + modelPath + "/" + modelId;
        }

        public string GetARModuleQRPath(string qrToken)
        {
            return serverName + armoduleQRPath + "/" + qrToken;
        }
    }
}

