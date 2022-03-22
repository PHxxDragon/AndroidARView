using System;
using UnityEngine;
using Vuforia;
using System.Collections;

namespace EAR.AR
{
    public class GroundPlaneController : MonoBehaviour
    {
        public enum GroundPlaneStateEnum
        {
            PlaneNotDetected, NotPlaced, Placed
        }

        public event Action<GroundPlaneStateEnum> OnStateChange;

        private GroundPlaneStateEnum currentState;

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

        private float time = 2f;

        private void SetState(GroundPlaneStateEnum state)
        {
            if (currentState != state)
            {
                currentState = state;
                if (isActiveAndEnabled)
                {
                    OnStateChange?.Invoke(state);
                }
            }
        }

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
            anchorBehaviour.OnTargetStatusChanged += OnAnchorTargetStatusChanged;
            planeFinderBehaviour.OnAutomaticHitTest.AddListener(OnAutomaticHitTest);
        }

        void OnEnable()
        {
            StartCoroutine(UpdateEvery1s());
        }

        private IEnumerator UpdateEvery1s()
        {
            while(true)
            {
                yield return new WaitForSecondsRealtime(1f);
                if (isActiveAndEnabled)
                {
                    time--;
                    if (time <= 0)
                    {
                        if (currentState != GroundPlaneStateEnum.Placed)
                        {
                            SetState(GroundPlaneStateEnum.PlaneNotDetected);
                        }
                    }
                }
            }
        }

        private void OnAutomaticHitTest(HitTestResult arg0)
        {
            if (currentState != GroundPlaneStateEnum.Placed)
            {
                SetState(GroundPlaneStateEnum.NotPlaced);
                time = 2f;
            }
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
            SetState(GroundPlaneStateEnum.PlaneNotDetected);
        }

        private void OnAnchorTargetStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
        {
            if (targetStatus.Status == Status.NO_POSE)
            {
                planeFinderBehaviour.enabled = true;
                SetState(GroundPlaneStateEnum.PlaneNotDetected);
            } else
            {
                planeFinderBehaviour.enabled = false;
                SetState(GroundPlaneStateEnum.Placed);
            }
        }

        private void OnTargetStatusChanged(ObserverBehaviour observerBehaviour, TargetStatus targetStatus)
        {
            if (targetStatus.Status == Status.LIMITED || targetStatus.Status == Status.NO_POSE)
            {
                SetState(GroundPlaneStateEnum.PlaneNotDetected);
            }
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
