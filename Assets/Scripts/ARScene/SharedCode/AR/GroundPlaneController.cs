using Vuforia.UnityRuntimeCompiled;
using UnityEngine;

namespace EAR.AR
{
    public class GroundPlaneController : MonoBehaviour
    {
        [SerializeField]
        private GameObject modelContainer;
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private GameObject raycastPlane;

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        void Update()
        {
            SetModelContainerToTouchPosition();
        }

        private void SetModelContainerToTouchPosition()
        {
            if (Input.GetMouseButton(0))
            {
                if (!UnityRuntimeCompiledFacade.Instance.IsUnityUICurrentlySelected())
                {
                    Ray cameraToPlaneRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(cameraToPlaneRay);
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.gameObject == raycastPlane)
                        {
                            modelContainer.transform.position = hit.point;
                        }
                    }
                }
            }
        }
    }

}
