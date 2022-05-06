using System;
using System.Collections.Generic;

namespace EAR
{
    [Serializable]
    public class AssetInformation
    {
        public int id;
        public string type;
        public string metadata;
        public string markerImage;
        public float markerImageWidth;
        public List<AssetObject> assets = new List<AssetObject>();

        public AssetInformation Copy()
        {
            AssetInformation assetInformation = new AssetInformation();
            assetInformation.metadata = metadata;
            assetInformation.markerImage = markerImage;
            assetInformation.markerImageWidth = markerImageWidth;
            foreach (AssetObject assetObject in assets)
            {
                assetInformation.assets.Add(assetObject.Copy());
            }
            return assetInformation;
        }
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
        public int size;

        //type == "model"
        public bool isZipFile;

        //type == "video"
        public bool predownload;

        public AssetObject Copy()
        {
            AssetObject assetObject = new AssetObject();
            assetObject.assetId = assetId;
            assetObject.name = name;
            assetObject.url = url;
            assetObject.type = type;
            assetObject.extension = extension;
            assetObject.size = size;
            assetObject.isZipFile = isZipFile;
            assetObject.predownload = predownload;
            return assetObject;
        }
    }
}

