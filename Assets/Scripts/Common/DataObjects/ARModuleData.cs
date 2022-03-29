using System;

namespace EAR
{
    [Serializable]
    public class ARModuleDataResponse
    {
        public ARModuleDataObject data;
    }

    [Serializable]
    public class ARModuleDataObject
    {
        public int id;
        public string name;
        public string description;
        public string metadata;
        public string image;
        public float markerImageWidth;
        public int modelId;
    }

    
}
