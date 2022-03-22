using System;
using System.Collections.Generic;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class MetadataObject
    {
        public TransformData modelTransform;
/*        public float imageWidthInMeters;*/
        public List<NoteData> noteDatas;
        public Color ambientColor = Color.white;
    }
}

