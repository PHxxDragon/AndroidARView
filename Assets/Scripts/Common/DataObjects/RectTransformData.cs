using UnityEngine;
using System;

namespace EAR
{
    [Serializable]
    public class RectTransformData
    {
        public Vector3 localPosition;
        public Vector2 anchoredPosition;
        public Vector2 sizeDelta;
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 pivot;
        public Vector3 localScale;
        public Quaternion localRotation;

        public static RectTransformData RectTransformToRectTransformData(RectTransform rectTransform)
        {
            RectTransformData rectTransformData = new RectTransformData();
            rectTransformData.localPosition = rectTransform.localPosition;
            rectTransformData.anchoredPosition = rectTransform.anchoredPosition;
            rectTransformData.sizeDelta = rectTransform.sizeDelta;
            rectTransformData.anchorMin = rectTransform.anchorMin;
            rectTransformData.anchorMax = rectTransform.anchorMax;
            rectTransformData.pivot = rectTransform.pivot;
            rectTransformData.localScale = rectTransform.localScale;
            rectTransformData.localRotation = rectTransform.localRotation;
            return rectTransformData;
        }

        public static void RectTransformDataToRectTransform(RectTransformData rectTransformData, RectTransform rectTransform)
        {
            rectTransform.localPosition = rectTransformData.localPosition;
            rectTransform.anchoredPosition = rectTransformData.anchoredPosition;
            rectTransform.sizeDelta = rectTransformData.sizeDelta;
            rectTransform.anchorMin = rectTransformData.anchorMin;
            rectTransform.anchorMax = rectTransformData.anchorMax;
            rectTransform.pivot = rectTransformData.pivot;
            rectTransform.localScale = rectTransformData.localScale;
            rectTransform.localRotation = rectTransformData.localRotation;
        }
    }
}

