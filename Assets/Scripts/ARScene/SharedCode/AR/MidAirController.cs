
using System;
using UnityEngine;
using Vuforia;

namespace EAR.AR
{
    public class MidAirController : MonoBehaviour
    {
        public event Action<MidAirStateEnum> OnStateChanged;
        public enum MidAirStateEnum
        {
            NoPose, NotPlaced, Placed
        }

        [SerializeField]
        private GameObject modelContainer;
        [SerializeField]
        private MidAirPositionerBehaviour midAirPositionerBehaviour;
        [SerializeField]
        private AnchorBehaviour anchorBehaviour;

        private MidAirStateEnum currentState;

        public void SetState(MidAirStateEnum state)
        {
            if (currentState != state)
            {
                currentState = state;
                if (isActiveAndEnabled)
                {
                    OnStateChanged?.Invoke(state);
                }
            }
        }

        void Start()
        {
            VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
            anchorBehaviour.OnTargetStatusChanged += OnAnchorTargetStatusChanged;
            CheckDevicePoseAndSetState();
        }

        void OnDestroy()
        {
            if (VuforiaBehaviour.Instance != null)
                VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
            if (anchorBehaviour != null)
                anchorBehaviour.OnTargetStatusChanged -= OnAnchorTargetStatusChanged;
        }

        private void CheckDevicePoseAndSetState()
        {
            Status deviceStatus = VuforiaBehaviour.Instance.DevicePoseBehaviour.TargetStatus.Status;
            if (deviceStatus == Status.NO_POSE || deviceStatus == Status.LIMITED)
            {
                SetState(MidAirStateEnum.NoPose);
            }
            else
            {
                SetState(MidAirStateEnum.NotPlaced);
            }
        }

        private void OnAnchorTargetStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
        {
            if (targetStatus.Status == Status.NO_POSE)
            {
                midAirPositionerBehaviour.enabled = true;
                CheckDevicePoseAndSetState();
            }
            else
            {
                midAirPositionerBehaviour.enabled = false;
                SetState(MidAirStateEnum.Placed);
            }
        }

        private void OnTargetStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
        {
            CheckDevicePoseAndSetState();
        }

        public void ResetTrackingStatus()
        {
            anchorBehaviour.UnconfigureAnchor();
            midAirPositionerBehaviour.enabled = true;
            CheckDevicePoseAndSetState();
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
                Bounds bounds = Utils.GetEntityBounds(modelContainer);
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

