
using UnityEngine;
using System;
using TriLibCore;
using Piglet;
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

        public GameObject GetModel()
        {
            return loadedModel;
        }
        /*        void Start()
                {
                    LoadModel("D:\\Mon hoc\\Nam 4 ky 1\\luan van\\EAR_OLD\\backend\\public\\wolf_with_animations.zip");
                }*/

        public void LoadModel(string url, string extension)
        {
            if (extension == "gltf" || extension == "glb")
            {
                LoadModelUsingPiglet(url);
            }
            else
            {
                LoadModelUsingTrilib(url, extension);
            }

        }

        //======================================piglet================================================

        private void LoadModelUsingPiglet(string url)
        {
            task = RuntimeGltfImporter.GetImportTask(url);
            task.OnCompleted = OnComplete;
            task.OnException += OnException;
            task.OnProgress += OnProgress;
            OnLoadStarted?.Invoke();
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
        }

        private void OnException(Exception e)
        {
            OnLoadError?.Invoke("Cannot load model");
        }

        //===========================================Trilib==========================================

        private void LoadModelUsingTrilib(string url, string extension)
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            var webRequest = AssetDownloader.CreateWebRequest(url);
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions, null, extension);
        }

        private void OnError(IContextualizedError obj)
        {
            OnLoadError?.Invoke("Cannot load model");
        }

        private void OnProgress(AssetLoaderContext arg1, float arg2)
        {
            OnLoadProgressChanged?.Invoke(arg2, "Loading...");
        }

        private void OnMaterialsLoad(AssetLoaderContext obj)
        {
            OnLoadEnded?.Invoke();
        }

        private void OnLoad(AssetLoaderContext obj)
        {
            loadedModel = obj.RootGameObject;
        }
    }
}