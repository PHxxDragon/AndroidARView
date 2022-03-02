using UnityEngine;
using Vuforia;

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
        [SerializeField]
        private PlaneFinderBehaviour planeFinderBehaviour;

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        public void PerformHitTest(Vector2 position)
        {
            planeFinderBehaviour.PerformHitTest(position);
        }

        public void SetModelContainerToTouchPosition(Vector2 position)
        {
            Ray cameraToPlaneRay = mainCamera.ScreenPointToRay(position);
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
