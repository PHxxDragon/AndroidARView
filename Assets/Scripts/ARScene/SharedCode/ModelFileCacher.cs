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
        private const string DICTIONARY_FILE_NAME = "DictFile.bin";
        private const string MODEL_FOLDER_NAME = "Models";

        private const string ENC_HEADER = "f7b025810bc4592b542544ac4b4fb0a501fc5e8fa49593297dca71de831f1787";
        private const int HEADER_LENGTH = 32;
        private const int IV_LENGTH = 16;
        private const int BUFFER_SIZE = 1024 * 1024;

        public void DownloadAndGetFileStream(string resourceUrl, string resourceKey, string resourceName, string resourceExtension, bool isZipFile, Action<byte[]> callback = null, Action<float> progressCallback = null, Action<string> errorCallback = null)
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

        private IEnumerator DownloadAndGetFilePathCoroutine(string resourceUrl, string resourceKey, string resourceName, string resourceExtension, bool isZipFile, Action<byte[]> callback = null, Action<float> progressCallback = null, Action<string> errorCallback = null)
        {
            Debug.Log("ResourceUrl: " + resourceUrl);
            string hashedFilename = GetHashString(resourceKey);
            string physicalExtension = isZipFile ? "zip" : resourceExtension;
            string filePath = GetModelFilePath(hashedFilename, physicalExtension);
            if (!File.Exists(filePath))
            {
                using UnityWebRequest uwr = UnityWebRequest.Get(resourceUrl);
                uwr.downloadHandler = new DownloadHandlerFile(filePath);
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
                    yield break;
                }
                else if (uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Protocol Error: " + uwr.error);
                    errorCallback?.Invoke(uwr.error);
                    yield break;
                }
                else
                {
                    Dictionary<string, (string, string)> dict = LoadDictionary();
                    if (!dict.ContainsKey(hashedFilename))
                    {
                        dict.Add(hashedFilename, (resourceName, physicalExtension));
                    }
                    SaveDictionary(dict);
                }
            }
            StartDecryptionThread(filePath, callback, progressCallback, errorCallback);
        }

        private void StartDecryptionThread(string filePath, Action<byte[]> callback, Action<float> progressCallback, Action<string> errorCallback)
        {
            Thread thread = new Thread(() =>
            {
                using (FileStream inputFile = File.OpenRead(filePath))
                {
                    byte[] header = Utils.StringToByteArrayFastest(ENC_HEADER);
                    byte[] test_header = new byte[HEADER_LENGTH];
                    inputFile.Read(test_header, 0, HEADER_LENGTH);
                    byte[] data;
                    if (Utils.ByteArrayCompare(test_header, header))
                    {
                        byte[] firstIV = new byte[IV_LENGTH];
                        inputFile.Read(firstIV, 0, IV_LENGTH);
                        byte[] secondIV = new byte[IV_LENGTH];
                        inputFile.Read(secondIV, 0, IV_LENGTH);
                        int streamLength = (int) inputFile.Length - HEADER_LENGTH - 2 * IV_LENGTH;
                        data = new byte[streamLength];

                        inputFile.Read(data, 0, streamLength);

                        MyCryptography myCryptography = new MyCryptography();
                        myCryptography.Decrypt(data , firstIV, secondIV);
                    } else
                    {
                        data = new byte[inputFile.Length];
                        Buffer.BlockCopy(test_header, 0, data, 0, HEADER_LENGTH);
                        inputFile.Read(data, HEADER_LENGTH, (int) inputFile.Length - HEADER_LENGTH);
                    }
                    actionsPool.Add(() =>
                    {
                        callback?.Invoke(data);
                    });
                }
            });
            thread.Start();
        }

        private class MyCryptography
        {

            private const string ENC_KEY = "74f8aa7ce7f9b769ba8c22ad541763b3c7704d377a3dc544a3359a1512903518";
            private const int ENC_SIZE = 1024 * 1024;
            public void Decrypt(byte[] buffer, byte[] firstIV, byte[] lastIV)
            {
                int length = buffer.Length;
                byte[] lastPart = length < ENC_SIZE ?
                    new byte[roundToDivisibleBy16(length)] : new byte[ENC_SIZE];
                Buffer.BlockCopy(buffer, (int)length - lastPart.Length, lastPart, 0, lastPart.Length);
                byte[] lastPartDecrypted = DecryptBuffer(lastPart, lastIV);
                Buffer.BlockCopy(lastPartDecrypted, 0, buffer, (int)length - lastPartDecrypted.Length, lastPartDecrypted.Length);

                byte[] firstPart = length < ENC_SIZE ?
                    new byte[roundToDivisibleBy16(length)] : new byte[ENC_SIZE];
                Buffer.BlockCopy(buffer, 0, firstPart, 0, firstPart.Length);
                byte[] firstPartDecrypted = DecryptBuffer(firstPart, firstIV);
                Buffer.BlockCopy(firstPartDecrypted, 0, buffer, 0, firstPartDecrypted.Length);
            }

            private long roundToDivisibleBy16(long number)
            {
                return (number >> 4) << 4;
            }

            private byte[] DecryptBuffer(byte[] buffer, byte[] iv)
            {
                using Aes aes = Aes.Create();
                aes.Key = Utils.StringToByteArrayFastest(ENC_KEY);
                aes.IV = iv;
                aes.Padding = PaddingMode.None;

                ICryptoTransform decryptor = aes.CreateDecryptor();
                using MemoryStream memoryStream = new MemoryStream();
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(buffer, 0, buffer.Length);
                }
                return memoryStream.ToArray();
            }
        }
    }
}
