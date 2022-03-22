using System;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class NoteData
    {
        public RectTransformData noteContentRectTransformData;

        public string noteContent;
        public Color textBackgroundColor;

        public Vector4 borderWidth;
        public Vector4 textBorderRadius;
        public Color borderColor;

        public int fontSize;
        public Color textColor;

        public float boxWidth;
        public float height;


        public TransformData noteTransformData;

        public string buttonTitle;
        public Color buttonBackgroundColor;

        public Vector4 buttonBorderWidth;
        public Vector4 buttonBorderRadius;
        public Color buttonBorderColor;

        public int buttonFontSize;
        public Color buttonTextColor;
    }
}

