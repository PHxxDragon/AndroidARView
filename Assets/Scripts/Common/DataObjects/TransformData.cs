using System;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public static TransformData TransformToTransformData(Transform transform)
        {
            TransformData transformData = new TransformData();
            transformData.position = transform.localPosition;
            transformData.rotation = transform.localRotation;
            transformData.scale = transform.localScale;
            return transformData;
        }

        public static void TransformDataToTransfrom(TransformData transformData, Transform transform)
        {
            transform.localPosition = transformData.position;
            transform.localRotation = transformData.rotation;
            transform.localScale = transformData.scale;
        }

        public static void ResetTransform(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }

        public static void SetParent(Transform child, Transform parent)
        {
            TransformData data = TransformToTransformData(child);
            child.parent = parent;
            TransformDataToTransfrom(data, child);
        } 
    }
}
