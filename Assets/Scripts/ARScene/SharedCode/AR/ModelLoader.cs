using UnityEngine;
using System;
using TriLibCore;
using Piglet;
using EAR.Cacher;
using System.IO;

namespace EAR.AR
{
    public class ModelLoader : MonoBehaviour
    {
        public event Action OnLoadStarted;
        public event Action OnLoadEnded;
        public event Action<string> OnLoadError;
        public event Action<float, string> OnLoadProgressChanged;

        private GameObject loadedModel;
        private GltfImportTask task;
        private string tempFilePath;

        [SerializeField]
        private ModelFileCacher modelFileCacher;

        public GameObject GetModel()
        {
            return loadedModel;
        }
        /*        void Start()
                {
                    LoadModel("D:\\Mon hoc\\Nam 4 ky 1\\luan van\\EAR_OLD\\backend\\public\\wolf_with_animations.zip");
                }*/

        public void LoadModel(string url, string name, string extension, bool isZipFile)
        {
            OnLoadStarted?.Invoke();
            modelFileCacher.DownloadAndGetFileStream(url, url, name, extension, isZipFile, (byte[] data) =>
            {
                if (extension == "gltf" || extension == "glb")
                {
                    LoadModelUsingPiglet(data);
                }
                else
                {
                    LoadModelUsingTrilib(data, extension, isZipFile);
                }
            }, 
            (float progress) =>
            {
                OnLoadProgressChanged?.Invoke(progress, string.Format("Downloading... {0}%", progress * 100));
            }, 
            (string message) =>
            {
                OnLoadError?.Invoke(message);
            });
            
        }

        //======================================piglet================================================

        private void LoadModelUsingPiglet(byte[] data)
        {
            task = RuntimeGltfImporter.GetImportTask(data);
            task.OnCompleted = OnComplete;
            task.OnException += OnException;
            task.OnProgress += OnProgress;
        }

        void Update()
        {
            if (task != null)
                task.MoveNext();
        }

        void OnProgress(GltfImportStep step, int completed, int total)
        {
            if (step == GltfImportStep.Download)
            {
                float downloadedMB = (float)completed / 1000000;
                if (total == 0)
                {
                    OnLoadProgressChanged?.Invoke(downloadedMB, string.Format("{0}: {1:0.00}MB", step, downloadedMB));
                }
                else
                {
                    float totalMB = (float)total / 1000000;
                    OnLoadProgressChanged?.Invoke(downloadedMB / totalMB, string.Format("{0}: {1:0.00}/{2:0.00}MB", step, downloadedMB, totalMB));
                }
            }
            else
            {
                OnLoadProgressChanged?.Invoke(((float)completed) / total, string.Format("{0}: {1}/{2}", step, completed, total));
            }
        }

        private void OnComplete(GameObject importedModel)
        {
            loadedModel = importedModel;
            OnLoadEnded?.Invoke();
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        private void OnException(Exception e)
        {
            OnLoadError?.Invoke("Cannot load model");
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        //===========================================Trilib==========================================

        private void LoadModelUsingTrilib(byte[] data, string extension, bool isZipFile)
        {
            using MemoryStream memoryStream = new MemoryStream(data);
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            if (isZipFile)
            {
                AssetLoaderZip.LoadModelFromZipStream(memoryStream, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
            } else
            {
                AssetLoader.LoadModelFromStream(memoryStream, "model", extension, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
            }
        }

        private void OnError(IContextualizedError obj)
        {
            OnLoadError?.Invoke("Cannot load model");
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        private void OnProgress(AssetLoaderContext arg1, float arg2)
        {
            OnLoadProgressChanged?.Invoke(arg2, Utils.GetLocalizedText("Loading..."));
        }

        private void OnMaterialsLoad(AssetLoaderContext obj)
        {
            OnLoadEnded?.Invoke();
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        private void OnLoad(AssetLoaderContext obj)
        {
            loadedModel = obj.RootGameObject;
        }
    }
}