using System;
using System.Collections.Generic;

namespace EAR
{
    [Serializable]
    public class AssetInformation
    {
        public string metadata;
        public string markerImage;
        public float markerImageWidth;
        public List<AssetObject> assets = new List<AssetObject>();
    }

    [Serializable]
    public class AssetObject
    {
        public const string MODEL_TYPE = "model";
        public const string IMAGE_TYPE = "image";
        public const string SOUND_TYPE = "sound";
        public const string FONT_TYPE = "font";
        public const string VIDEO_TYPE = "video";

        public string assetId;
        public string name;
        public string url;
        public string type;
        public string extension;

        //type == "model"
        public bool isZipFile;
    }
}

