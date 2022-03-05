using System;

namespace EAR
{
    [Serializable]
    public class NoteData
    {
        public RectTransformData noteContentRectTransformData;
        public string noteContent;
        public string buttonTitle;
        public TransformData noteTransformData;
        public int fontSize;
        public float boxWidth;
        public float height;
    }
}

