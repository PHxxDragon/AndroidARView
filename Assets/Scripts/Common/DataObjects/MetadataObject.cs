using System;
using System.Collections.Generic;

namespace EAR
{
    [Serializable]
    public class MetadataObject
    {
        public TransformData modelTransform;
        public float imageWidthInMeters;
        public List<NoteData> noteDatas;
    }
}

