using UnityEngine;
using Unity.RemoteConfig;

namespace EAR
{
    [CreateAssetMenu(menuName = "EAR/Configuration")]
    public class ApplicationConfiguration : ScriptableObject
    {
        [SerializeField]
        private string localServerName;

        [SerializeField]
        private string loginPath;

        [SerializeField]
        private string profilePath;

        [SerializeField]
        private string armodulePath;

        [SerializeField]
        private string modelPath;

        [SerializeField]
        private string modelListPath;

        [SerializeField]
        private string qrCodePath;

        [SerializeField]
        private string categoriesPath;

        [SerializeField]
        private string CourseListPath;

        [SerializeField]
        private string coursesPath;

        [SerializeField]
        private string moduleListPath;

        private string serverName;

        public struct UserAttributes { }
        public struct AppAttributes { }

        void Awake()
        {
            serverName = localServerName;
            //ConfigManager.FetchCompleted += ApplyRemoteSettings;
            serverName = "http://192.168.1.8:3000/";
            ConfigManager.FetchConfigs(new UserAttributes(), new AppAttributes());
        }

        private void ApplyRemoteSettings(ConfigResponse obj)
        {
            serverName = ConfigManager.appConfig.GetString("ServerName");
/*            Debug.Log("Loaded Server Name " + serverName);*/
        }

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

        public string GetARModulePath(int moduleId)
        {
            return serverName + armodulePath + "/" + moduleId;
        }

        public string GetModelListPath(int page, int limit, string filter, string keyword)
        {
            string typeQuery = string.IsNullOrEmpty(filter) ? "" : "&type=" + filter;
            string keywordQuery = string.IsNullOrEmpty(keyword) ? "" : "&keyword=" + keyword;
            return serverName + modelPath + "/" + modelListPath + "?page=" + page + "&limit=" + limit +  typeQuery + keywordQuery;
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

        public string GetCourseListPath(int page, int limit, string type, string keyword)
        {
            string typeQuery = string.IsNullOrEmpty(type) ? "" : "&type=" + type;
            string keywordQuery = string.IsNullOrEmpty(keyword) ? "" : "&keyword=" + keyword;
            return serverName + CourseListPath + "?page=" + page + "&limit=" + limit + typeQuery + keywordQuery;
        }

        public string GetModuleListPath(int arModule)
        {
            return serverName + coursesPath + "/" + arModule + "/" + moduleListPath;
        }

        void OnDestroy()
        {
            ConfigManager.FetchCompleted -= ApplyRemoteSettings;
        }
    }
}

