using UnityEngine;
using EAR.EARTouch;
using EAR.AR;

namespace EAR.Presenter
{
    public class TouchHandlerPresenter : MonoBehaviour
    {
        public float rotateSensitivity = 1f;

        [SerializeField]
        private TouchHandler touchHandler;
        [SerializeField]
        private GroundPlaneController groundPlaneController;
        [SerializeField]
        private MidAirController midAirController;
        [SerializeField]
        private GameObject modelContainer;

        private float startDistance;
        private Vector3 startLocalScale;

        private float startAngle;
        private Quaternion startLocalRotation;

        void Start()
        {
            if (touchHandler == null || modelContainer == null || groundPlaneController == null)
            {
                Debug.Log("Unassigned references");
                return;
            }

            touchHandler.OnTouchPinchStart += (float startDistance) =>
            {
                this.startDistance = startDistance;
                startLocalScale = modelContainer.transform.localScale;
            };
            touchHandler.OnTouchPinchChange += (float distance) =>
            {
                if (modelContainer.activeSelf)
                {
                    modelContainer.transform.localScale = startLocalScale * distance / startDistance;
                }
            };
            touchHandler.OnTouchRotationStart += (float startAngle) =>
            {
                this.startAngle = startAngle;
                startLocalRotation = modelContainer.transform.localRotation;
            };
            touchHandler.OnTouchRotationChange += (float angle) =>
            {
                if (modelContainer.activeSelf)
                {
                    modelContainer.transform.localRotation = Quaternion.Euler(0, -(angle - startAngle) * rotateSensitivity, 0) * startLocalRotation;
                }
            };
            touchHandler.OnSingleFingerDragStart += (Vector2 position) =>
            {
                groundPlaneController.SetModelContainerToTouchPosition(position);
            };
            touchHandler.OnSingleFingerDragChange += (Vector2 position) =>
            {
                groundPlaneController.SetModelContainerToTouchPosition(position);
            };
            touchHandler.OnSingleFingerClick += (Vector2 position) =>
            {
                if (groundPlaneController != null && groundPlaneController.isActiveAndEnabled)
                {
                    groundPlaneController.PerformHitTest(position);
                } else if (midAirController != null && midAirController.isActiveAndEnabled)
                {
                    midAirController.PerformClick(position);
                }
            };
        }
    }
}

