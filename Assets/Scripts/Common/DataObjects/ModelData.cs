using System;

namespace EAR
{
    [Serializable]
    public class ModelDataResponse
    {
        public ModelDataObject data;
    }
    
    [Serializable]
    public class ModelDataObject
    {
        public string url;
        public string extension;
    }
}

