using System;

namespace EAR
{
    [Serializable]
    public class ARInformationResponse
    {
        public ARInformation data;
    }

    [Serializable]
    public class ARInformation
    {
        public string imageUrl;
        public string modelUrl;
        public string metadataString;
        public string extension;
        public bool isZipFile;
        public string name;
        public float markerImageWidth;
    }

}
