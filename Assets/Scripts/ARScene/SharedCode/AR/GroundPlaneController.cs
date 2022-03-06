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
        [SerializeField]
        private AnchorBehaviour anchorBehaviour;

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
            anchorBehaviour.OnTargetStatusChanged += OnAnchorTargetStatusChanged;
        }

        void OnDestroy()
        {
            if (VuforiaBehaviour.Instance != null)
                VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
            if (anchorBehaviour != null)
                anchorBehaviour.OnTargetStatusChanged -= OnAnchorTargetStatusChanged;
        }

        public void ResetTrackingStatus()
        {
            anchorBehaviour.UnconfigureAnchor();
            planeFinderBehaviour.enabled = true;
        }

        private void OnAnchorTargetStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
        {
            if (targetStatus.Status == Status.NO_POSE)
            {
                planeFinderBehaviour.enabled = true;
            } else
            {
                planeFinderBehaviour.enabled = false;
            }
        }

        private void OnTargetStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
        {
            Debug.Log("werthg In ground plane controller, device pose status: " + targetStatus.Status + " device pose statusinfo: " + targetStatus.StatusInfo);
        }

        public void PerformHitTest(Vector2 position)
        {
            if (planeFinderBehaviour.isActiveAndEnabled)
            {
                planeFinderBehaviour.PerformHitTest(position);
            }
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
