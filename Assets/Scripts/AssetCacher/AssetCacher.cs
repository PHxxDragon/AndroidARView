using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace EAR.AssetCache
{
    public class AssetCacher : MonoBehaviour
    {
        public const string MAX_STORAGE_KEY = "maxStorage";
        public const long DEFAULT_MAX_STORAGE = 1024 * 1024 * 1024;

        private const string CACHE_METADATA_FILENAME = "metadata.bin";
        private const string CACHE_METADATA_FOLDER = "meta";

        private const string CACHE_PARENT_FOLDER = "cache";

        private Dictionary<string, AssetCacheMetadata> cacheMetadata;

        void Awake()
        {
            cacheMetadata = LoadDictionary();
        }

        void OnDestroy()
        {
            SaveDictionary(cacheMetadata);
        }

        private void SaveDictionary(Dictionary<string, AssetCacheMetadata> metadata)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (!Directory.Exists(GetCacheMetadataFolderPath()))
            {
                Directory.CreateDirectory(GetCacheMetadataFolderPath());
            }
            using FileStream fileStream = File.Open(GetCacheMetadataFilePath(), FileMode.Create);
            binaryFormatter.Serialize(fileStream, metadata);
        }

        private Dictionary<string, AssetCacheMetadata> LoadDictionary()
        {
            if (File.Exists(GetCacheMetadataFilePath()))
            {
                using FileStream fileStream = File.Open(GetCacheMetadataFilePath(), FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (Dictionary<string, AssetCacheMetadata>)binaryFormatter.Deserialize(fileStream);
            } else
            {
                return new Dictionary<string, AssetCacheMetadata>();
            }
        }

        private string GetCacheFolderPath(string moduleId)
        {
            return Path.Combine(Application.persistentDataPath, CACHE_PARENT_FOLDER, moduleId);
        }

        private string GetCacheParentPath()
        {
            return Path.Combine(Application.persistentDataPath, CACHE_PARENT_FOLDER);
        }

        private string GetCacheMetadataFolderPath()
        {
            return Path.Combine(Application.persistentDataPath, CACHE_METADATA_FOLDER);
        }

        private string GetCacheMetadataFilePath()
        {
            return Path.Combine(Application.persistentDataPath, CACHE_METADATA_FOLDER, CACHE_METADATA_FILENAME);
        }

        public static string GetIdFromTypeAndId(int id, string type)
        {
            return type + "_" + id;
        }

        public bool NeedDownload(string moduleId)
        {
            return false;
        }

        public bool HasCache(string moduleId)
        {
            return false;
        }

        public void SetPreserve(string moduleId, bool preserve)
        {
            if(cacheMetadata.TryGetValue(moduleId, out AssetCacheMetadata assetCache))
            {
                assetCache.preserve = preserve;
            }
        }

        public long GetCurrentStorage()
        {
            return cacheMetadata.Sum((assetCache) => assetCache.Value.cacheSize);
        }

        public long GetMaxStorage()
        {
            string maxStorageString = LocalStorage.Load(MAX_STORAGE_KEY);
            return string.IsNullOrEmpty(maxStorageString) ? DEFAULT_MAX_STORAGE : long.Parse(maxStorageString);
        }

        public void SetMaxStorage(long value)
        {
            LocalStorage.Save(MAX_STORAGE_KEY, value.ToString());
        }

        public void ProcessCache(string moduleId, AssetInformation assetInformation,Action<AssetInformation> callback = null, Action<string> errorCallback = null, Action<float> progressCallback = null)
        {
            long maxStorage = GetMaxStorage();

            AssetInformation newInformation = assetInformation.Copy();
            // create new cache metadata if not exists
            if (!cacheMetadata.ContainsKey(moduleId))
            {
                cacheMetadata.Add(moduleId, new AssetCacheMetadata());
            }

            // calculate recent time and cacheSize
            cacheMetadata[moduleId].recentAccessTime = DateTime.Now;
            cacheMetadata[moduleId].cacheSize = assetInformation.assets.Sum(asset => asset.size);

            // check if there is any redundant file in cache and delete it
            HashSet<string> urlSet = new HashSet<string>();
            foreach (AssetObject asset in newInformation.assets)
            {
                urlSet.Add(asset.url);
            }

            List<string> redundant = new List<string>();
            foreach (string url in cacheMetadata[moduleId].urlToLocalDict.Keys)
            {
                if (!urlSet.Contains(url))
                {
                    redundant.Add(url);
                }
            }

            foreach(string url in redundant)
            {
                if (File.Exists(cacheMetadata[moduleId].urlToLocalDict[url]))
                {
                    File.Delete(cacheMetadata[moduleId].urlToLocalDict[url]);
                    cacheMetadata[moduleId].urlToLocalDict.Remove(url);
                }
                else
                {
                    Debug.LogError("File do not exist but metadata has it");
                }
            }

            // Check if enough storage for caching and remove if necessary
            long totalCacheStorage = cacheMetadata.Values.Sum(assetCache => assetCache.cacheSize);
            while (totalCacheStorage >= maxStorage)
            {
                KeyValuePair<string, AssetCacheMetadata> oldest = cacheMetadata.Aggregate((obj1, obj2) => obj1.Value.recentAccessTime < obj2.Value.recentAccessTime ? obj1 : obj2);
                totalCacheStorage -= oldest.Value.cacheSize;
                ClearCache(oldest.Key);
                cacheMetadata.Remove(oldest.Key);
            }

            //Start caching
            AssetCacheProgressInfo progressInfo = new AssetCacheProgressInfo();
            progressInfo.assetCount = newInformation.assets.Count;

            foreach (AssetObject asset in newInformation.assets)
            {
                if (asset.type == AssetObject.VIDEO_TYPE && !asset.predownload)
                {
                    progressInfo.assetCount -= 1;
                    if (progressInfo.assetCount == 0 && !progressInfo.hasError)
                    {
                        callback?.Invoke(newInformation);
                    }
                    continue;
                }

                if (cacheMetadata[moduleId].urlToLocalDict.ContainsKey(asset.url))
                {
                    asset.url = cacheMetadata[moduleId].urlToLocalDict[asset.url];
                    progressInfo.assetCount -= 1;
                    if (progressInfo.assetCount == 0 && !progressInfo.hasError)
                    {
                        callback?.Invoke(newInformation);
                    }

                    continue;
                }

                Utils.Instance.GetFile(asset.url, asset.extension, GetCacheFolderPath(moduleId), 
                (url) =>
                {
                    cacheMetadata[moduleId].urlToLocalDict.Add(asset.url, url);
                    asset.url = new Uri(url).AbsoluteUri;
                    progressInfo.assetCount -= 1;
                    if (progressInfo.assetCount == 0 && !progressInfo.hasError)
                    {
                        callback?.Invoke(newInformation);
                    }
                }, 
                (error) =>
                {
                    progressInfo.hasError = true;
                    errorCallback?.Invoke(error + " asset name: " + asset.name);
                }, 
                (progress) =>
                {
                    if (!progressInfo.hasError)
                    {
                        progressCallback?.Invoke(progress);
                    }
                });
             }
        }

        public void ClearCache(string moduleId)
        {
            if (Directory.Exists(GetCacheFolderPath(moduleId)))
            {
                Directory.Delete(GetCacheFolderPath(moduleId), true);
            }

            if (cacheMetadata.ContainsKey(moduleId))
            {
                cacheMetadata.Remove(moduleId);
                SaveDictionary(cacheMetadata);
            }
        }

        public void ClearAllCache()
        {
            if (Directory.Exists(GetCacheParentPath()))
            {
                Directory.Delete(GetCacheParentPath(), true);
            }

            cacheMetadata.Clear();
            SaveDictionary(cacheMetadata);
        }
    }
}

