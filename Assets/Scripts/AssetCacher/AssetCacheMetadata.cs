using System;
using System.Collections.Generic;

namespace EAR.AssetCache
{
    [Serializable]
    public class AssetCacheMetadata
    {
        public Dictionary<string, string> urlToLocalDict = new Dictionary<string, string>();
        public DateTime recentAccessTime;
        public bool preserve;
        public long cacheSize;
    }

    [Serializable]
    public class AssetCacheProgressInfo
    {
        public int assetCount;
        public bool hasError;
    }

}
