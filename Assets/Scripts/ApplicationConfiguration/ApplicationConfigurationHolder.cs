using UnityEngine;

namespace EAR
{
    public class ApplicationConfigurationHolder : MonoBehaviour
    {
        [SerializeField]
        private ApplicationConfiguration applicationConfiguration;

        private static ApplicationConfigurationHolder instance;

        public static ApplicationConfigurationHolder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<ApplicationConfigurationHolder>();
                    instance.applicationConfiguration = Resources.Load<ApplicationConfiguration>("ApplicationConfiguration");
                    Debug.Log(instance.applicationConfiguration.GetServerName());
                }
                return instance;
            }
        }

        public ApplicationConfiguration GetApplicationConfiguration()
        {
            return applicationConfiguration;
        }
    }
}

