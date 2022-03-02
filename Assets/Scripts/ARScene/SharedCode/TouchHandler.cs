using System;
using UnityEngine;
using Vuforia.UnityRuntimeCompiled;

namespace EAR.EARTouch
{
    public class TouchHandler : MonoBehaviour
    {
        public enum TouchGesture
        {
            Undefined, Pinch, Rotate
        }

        public event Action<float> OnTouchRotationStart;
        public event Action<float> OnTouchRotationChange;
        public event Action OnTouchRotationEnd;

        public event Action<float> OnTouchPinchStart;
        public event Action<float> OnTouchPinchChange;
        public event Action OnTouchPinchEnd;

        public event Action<Vector2> OnSingleFingerDragStart;
        public event Action<Vector2> OnSingleFingerDragChange;
        public event Action OnSingleFingerDragEnd;

        public event Action<Vector2> OnSingleFingerClick;

        public float deltaAngleForRotate = 0.5f;
        public float deltaDistanceForPinch = 5f;
        public float deltaTimeForSingleFinger = 0.3f;

        private TouchGesture currentTouchGesture;
        private float initTouchAngle;
        private float initTouchDistance;
        private bool isFirstFrameWithTwoTouch = true;

        private int lastTouchCountNum;
        private float timer = 0;
        private bool isSingleFingerDrag;

        private bool isSingleFingerClick;
        private Vector2 clickPosition;

        void Update()
        {
            if (Input.touchCount == 2)
            {
                float touchAngle = GetTouchAngle(Input.GetTouch(0), Input.GetTouch(1));
                float touchDistance = GetTouchDistance(Input.GetTouch(0), Input.GetTouch(1));
                if (isFirstFrameWithTwoTouch)
                {
                    initTouchAngle = touchAngle;
                    initTouchDistance = touchDistance;
                    isFirstFrameWithTwoTouch = false;
                }
                else
                {
                    float deltaAngle = Mathf.Abs(touchAngle - initTouchAngle);
                    float deltaDistance = Mathf.Abs(touchDistance - initTouchDistance);
                    switch (currentTouchGesture)
                    {
                        case TouchGesture.Undefined:
                            if (deltaAngle > deltaAngleForRotate)
                            {
                                currentTouchGesture = TouchGesture.Rotate;
                                OnTouchRotationStart?.Invoke(initTouchAngle);
                            }
                            else if (deltaDistance > deltaDistanceForPinch)
                            {
                                currentTouchGesture = TouchGesture.Pinch;
                                OnTouchPinchStart?.Invoke(initTouchDistance);
                            }
                            break;
                        case TouchGesture.Pinch:
                            OnTouchPinchChange?.Invoke(touchDistance);
                            break;
                        case TouchGesture.Rotate:
                            OnTouchRotationChange?.Invoke(touchAngle);
                            break;
                    }
                }
            }
            else
            {
                if (currentTouchGesture != TouchGesture.Undefined)
                {
                    if (currentTouchGesture == TouchGesture.Pinch)
                    {
                        OnTouchPinchEnd?.Invoke();
                    }
                    else
                    {
                        OnTouchRotationEnd?.Invoke();
                    }
                    currentTouchGesture = TouchGesture.Undefined;
                    isFirstFrameWithTwoTouch = true;
                }
            }

            if (Input.touchCount == 1 && lastTouchCountNum == 0 && !UnityRuntimeCompiledFacade.Instance.IsUnityUICurrentlySelected())
            {
                if (!isSingleFingerClick)
                {
                    isSingleFingerClick = true;
                    clickPosition = Input.GetTouch(0).position;
                }


                if (!isSingleFingerDrag)
                {
                    if (timer >= deltaTimeForSingleFinger)
                    {
                        isSingleFingerDrag = true;
                        OnSingleFingerDragStart?.Invoke(Input.GetTouch(0).position);
                    }
                }
                else
                {
                    OnSingleFingerDragChange?.Invoke(Input.GetTouch(0).position);
                }
                timer += Time.deltaTime;
            }
            else
            {
                if (Input.touchCount == 0 && isSingleFingerClick && !isSingleFingerDrag)
                {
                    OnSingleFingerClick?.Invoke(clickPosition); 
                }
                isSingleFingerClick = false;

                if (isSingleFingerDrag)
                {
                    OnSingleFingerDragEnd?.Invoke();
                    isSingleFingerDrag = false;
                }
                timer = 0;
            }

            if (Input.touchCount == 0 || Input.touchCount >= 2)
            {
                lastTouchCountNum = Input.touchCount;
            }

        }

        private float GetTouchAngle(Touch touch1, Touch touch2)
        {
            var diffY = touch1.position.y - touch2.position.y;
            var diffX = touch1.position.x - touch2.position.x;
            return Mathf.Atan2(diffY, diffX) * Mathf.Rad2Deg;
        }

        private float GetTouchDistance(Touch touch1, Touch touch2)
        {
            return Vector2.Distance(touch1.position, touch2.position);
        }
    }
}

