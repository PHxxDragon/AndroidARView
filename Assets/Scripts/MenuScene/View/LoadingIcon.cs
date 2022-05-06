using UnityEngine;

namespace EAR.View
{
    public class LoadingIcon : MonoBehaviour
    {
        [SerializeField]
        private GameObject loadingIcon;

        void Update()
        {
            Vector3 euler = loadingIcon.transform.rotation.eulerAngles;
            euler.z -= 10f;
            loadingIcon.transform.rotation = Quaternion.Euler(euler);
        }
    }
}

