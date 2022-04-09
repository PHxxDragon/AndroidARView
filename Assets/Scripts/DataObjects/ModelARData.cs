using System;

namespace EAR
{
    [Serializable]
    public class ModelARDataResponse
    {
        public ModelARDataObject data;
    }

    [Serializable]
    public class ModelARDataObject
    {
        public string markerImage;
        public float markerImageWidth;
        public string metadata;
    }
}
