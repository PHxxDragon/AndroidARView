using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Text;

namespace EAR.Cacher
{
    public class ModelFileCacher : MonoBehaviour
    {
        private ConcurrentBag<Action> actionsPool = new ConcurrentBag<Action>();
        private Dictionary<string, string> fileKeyToFileName;
        private const string DICTIONARY_FILE_NAME = "DictFile.bin";
        private const string MODEL_FOLDER_NAME = "Models";

        public void DownloadAndGetFilePath(string resourceUrl, string resourceKey, string resourceName, string resourceExtension, bool isZipFile, Action<string> callback = null, Action<float> progressCallback = null, Action<string> errorCallback = null)
        {
            StartCoroutine(DownloadAndGetFilePathCoroutine(resourceUrl, resourceKey, resourceName, resourceExtension, isZipFile, callback, progressCallback, errorCallback));
        }

        public Dictionary<string, (string, FileInfo)> GetFileDictionary()
        {
            Dictionary<string, (string, string)> dict = LoadDictionary();
            Dictionary<string, (string, FileInfo)> result = new Dictionary<string, (string, FileInfo)>();
            foreach (var keyValue in dict)
            {
                FileInfo fileInfo = new FileInfo(GetModelFilePath(keyValue.Key, keyValue.Value.Item2));
                result.Add(keyValue.Key, (keyValue.Value.Item1, fileInfo));
            }
            return result;
        }

        public void RemoveFile(string hashedFileName)
        {
            Debug.Log("Remove file " + hashedFileName);
            Dictionary<string, (string, string)> dict = LoadDictionary();
            (string, string) t;
            dict.TryGetValue(hashedFileName, out t);
            dict.Remove(hashedFileName);
            File.Delete(GetModelFilePath(hashedFileName, t.Item2));
            SaveDictionary(dict);
        }

        private string GetModelFilePath(string hashedFilename, string extension)
        {
            return string.Format("{0}/{1}/{2}.{3}", Application.persistentDataPath, MODEL_FOLDER_NAME, hashedFilename, extension);
        }

        private string GetDictionaryPath()
        {
            return Application.persistentDataPath + "/" + DICTIONARY_FILE_NAME;
        }

        void Update()
        {
            if (actionsPool.Count != 0)
            {
                Action action;
                if (actionsPool.TryTake(out action))
                {
                    action?.Invoke();
                }
            }
        }

        private string GetHashString(string inputString)
        {
            using HashAlgorithm algorithm = SHA256.Create();
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        private Dictionary<string, (string, string)> LoadDictionary()
        {
            string path = GetDictionaryPath();
            if (File.Exists(path))
            {
                using FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Dictionary<string, (string, string)> dict = (Dictionary<string, (string, string)>)binaryFormatter.Deserialize(stream);
                return dict;
            }
            else
            {
                Debug.Log("Create new dictionary");
                return new Dictionary<string, (string, string)>();
            }
        }

        private void SaveDictionary(Dictionary<string, (string, string)> dict)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = GetDictionaryPath();
            using FileStream stream = new FileStream(path, FileMode.Create);
            binaryFormatter.Serialize(stream, dict);
        }

        private IEnumerator DownloadAndGetFilePathCoroutine(string resourceUrl, string resourceKey, string resourceName, string resourceExtension, bool isZipFile, Action<string> callback = null, Action<float> progressCallback = null, Action<string> errorCallback = null)
        {
            Debug.Log("ResourceUrl: " + resourceUrl);
            string hashedFilename = GetHashString(resourceKey);
            string physicalExtension = isZipFile ? "zip" : resourceExtension;
            string filePath = GetModelFilePath(hashedFilename, physicalExtension);
            if (File.Exists(filePath))
            {
                Debug.Log("Detected file in system");
                callback?.Invoke(filePath);
                yield break;
            }

            using UnityWebRequest uwr = UnityWebRequest.Get(resourceUrl);
            UnityWebRequestAsyncOperation operation = uwr.SendWebRequest();
            while (!operation.isDone)
            {
                progressCallback?.Invoke(uwr.downloadProgress);
                yield return null;
            }
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection Error: " + uwr.error);
                errorCallback?.Invoke("Connection error");
            }
            else if (uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Protocol Error: " + uwr.error);
                errorCallback?.Invoke(uwr.error);
            }
            else
            {
                byte[] data = uwr.downloadHandler.data;
                StartWriteFileThread(filePath, data, () =>
                {
                    Dictionary<string, (string, string)> dict = LoadDictionary();
                    if (!dict.ContainsKey(hashedFilename))
                    {
                        dict.Add(hashedFilename, (resourceName, physicalExtension));
                    }
                    SaveDictionary(dict);
                    callback?.Invoke(filePath);
                });
            }
        }

        private void StartWriteFileThread(string path, byte[] data, Action callback = null)
        {
            Thread thread = new Thread(() =>
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllBytes(path, data);
                actionsPool.Add(callback);
            });
            thread.Start();
        }

    }
}
