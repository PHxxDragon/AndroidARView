
using System;
using UnityEngine;
using Vuforia;

namespace EAR.AR
{
    public class MidAirController : MonoBehaviour
    {
        [SerializeField]
        private GameObject modelContainer;
        [SerializeField]
        private MidAirPositionerBehaviour midAirPositionerBehaviour;
        [SerializeField]
        private AnchorBehaviour anchorBehaviour;

        void Start()
        {
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

        private void OnAnchorTargetStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
        {
            if (targetStatus.Status == Status.NO_POSE)
            {
                midAirPositionerBehaviour.enabled = true;
            }
            else
            {
                midAirPositionerBehaviour.enabled = false;
            }
        }

        private void OnTargetStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
        {
            Debug.Log("werthg In mid air controller, device pose status: " + targetStatus.Status + " device pose statusinfo: " + targetStatus.StatusInfo);
        }

        public void ResetTrackingStatus()
        {
            anchorBehaviour.UnconfigureAnchor();
            midAirPositionerBehaviour.enabled = true;
        }

        public void PerformClick(Vector2 position)
        {
            if (midAirPositionerBehaviour.isActiveAndEnabled)
            {
                midAirPositionerBehaviour.ConfirmAnchorPosition(position);
            }
        }

        public void AdjustModelPosition()
        {
            if (modelContainer != null)
            {
                Bounds bounds = Utils.GetModelBounds(modelContainer);
                if (bounds.size == Vector3.zero)
                {
                    return;
                }
                modelContainer.transform.localPosition = new Vector3(0, -bounds.center.y, 0);
            } else
            {
                Debug.Log("Unassigned references");
            }
        }
    }
}

