using Vuforia;
using UnityEngine;
using System;

namespace EAR.AR
{
    public class ImageTargetCreator : MonoBehaviour
    {
        public event Action CreateTargetDoneEvent;
        public event Action<string> CreateTargetErrorEvent;
        public event Action CreateTargetStartEvent;
        public event Action<float, string> OnProgressChanged;

        public static string IMAGE_FORMAT_ERROR = "ImageFormatError";

        private GameObject target;

        private float widthInMeter;

        public GameObject GetImageTarget()
        {
            return target;
        }
        public void CreateImageTarget(string url, float widthInMeter = 0.1f)
        {
            CreateTargetStartEvent?.Invoke();
            Utils.Instance.GetImageAsTexture2D(url, CreateTarget, CreateTargetError, 
                (float arg1, string arg2) => { OnProgressChanged?.Invoke(arg1, arg2); });
            this.widthInMeter = widthInMeter;
        }

        private void CreateTargetError(string arg1)
        {
            CreateTargetErrorEvent?.Invoke(arg1);
        }

        private void CreateTarget(Texture2D image)
        {
            Debug.Log(image);
            ImageTargetBehaviour mTarget;
            try
            {
                mTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(image, widthInMeter, "ImageTarget");
            } catch (ArgumentException)
            {
                Debug.LogError("Error creating image target with image format " + image.format);
                CreateTargetErrorEvent?.Invoke(IMAGE_FORMAT_ERROR);
                return;
            }
            
            mTarget.gameObject.AddComponent<DefaultObserverEventHandler>();

            //modelContainerHandler.gameObject.transform.parent = mTarget.gameObject.transform;

            // Hide the object
            mTarget.gameObject.transform.position = new Vector3(0, -10, 0);
            target = mTarget.gameObject;
            CreateTargetDoneEvent?.Invoke();
        }
    }
}

