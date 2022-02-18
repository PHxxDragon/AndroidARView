using Vuforia;
using UnityEngine;
using System.Collections;

namespace EAR.AR
{
    public class CameraSettings : MonoBehaviour
    {
        private bool hasVuforiaStarted = false;
        private bool isAutoFocusEnabled = true;
        private bool isFocusing = false;

        public static bool DoubleTap
        {
            get { return Input.touchSupported && (Input.touches.Length > 0) && (Input.touches[0].tapCount == 2); }
        }

        void Start()
        {
            VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
        }

        void Update()
        {
            if (DoubleTap && !isFocusing)
            {
                isFocusing = true;
                TriggerAutofocusEvent();
            }
        }

        void OnPaused(bool paused)
        {
            var appResumed = !paused;
            if (appResumed && hasVuforiaStarted)
            {
                // Restore original focus mode when app is resumed
                if (isAutoFocusEnabled)
                    VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
                else
                    VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_NORMAL);
            }
            else
            {
                // Set the torch flag to false on pause (because the flash torch is switched off by the OS automatically)
                isAutoFocusEnabled = false;
            }
        }

        private void TriggerAutofocusEvent()
        {
            // Trigger an autofocus event
            VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_TRIGGERAUTO);

            // Then restore original focus mode
            StartCoroutine(RestoreOriginalFocusMode());
        }

        private IEnumerator RestoreOriginalFocusMode()
        {
            yield return new WaitForSeconds(1.5f);

            // Restore original focus mode
            if (isAutoFocusEnabled)
                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            else
                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_NORMAL);
            isFocusing = false;
        }

        private void OnVuforiaStarted()
        {
            hasVuforiaStarted = true;
            // Try enabling continuous autofocus
            SwitchAutofocus(true);
        }

        private void SwitchAutofocus(bool on)
        {
            if (on)
            {
                if (VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
                {
                    Debug.Log("Successfully enabled continuous autofocus.");
                    isAutoFocusEnabled = true;
                }
                else
                {
                    // Fallback to normal focus mode
                    Debug.Log("Failed to enable continuous autofocus, switching to normal focus mode");
                    isAutoFocusEnabled = false;
                    VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_NORMAL);
                }
            }
            else
            {
                Debug.Log("Disabling continuous autofocus (enabling normal focus mode).");
                isAutoFocusEnabled = false;
                VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_NORMAL);

            }
        }
    }
}

