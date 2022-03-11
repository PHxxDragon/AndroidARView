using Vuforia;
using UnityEngine;
using System;

namespace EAR.AR
{
    public class ImageTargetCreator : MonoBehaviour
    {
        public event Action CreateTargetDoneEvent;
        public event Action<string> CreateTargetErrorEvent;

        private GameObject target;

        private float widthInMeter;

        public GameObject GetImageTarget()
        {
            return target;
        }
        public void CreateImageTarget(string url, float widthInMeter = 0.1f)
        {
            Utils.Instance.GetImageAsTexture2D(url, CreateTarget, CreateTargetError);
            this.widthInMeter = widthInMeter;
        }

        private void CreateTargetError(string arg1, object arg2)
        {
            CreateTargetErrorEvent?.Invoke(arg1);
        }

        private void CreateTarget(Texture2D image, object param)
        {
            Debug.Log(image);
            var mTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(image, widthInMeter, "ImageTarget");
            mTarget.gameObject.AddComponent<DefaultObserverEventHandler>();

            //modelContainerHandler.gameObject.transform.parent = mTarget.gameObject.transform;

            // Hide the object
            mTarget.gameObject.transform.position = new Vector3(0, -10, 0);
            target = mTarget.gameObject;
            CreateTargetDoneEvent?.Invoke();
        }
    }
}

