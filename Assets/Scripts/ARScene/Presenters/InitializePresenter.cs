using UnityEngine;
using System;
using EAR.View;
using EAR.Container;
using EAR.SceneChange;
using Vuforia;
using EAR.AR;


namespace EAR.Presenter
{
    public class InitializePresenter : MonoBehaviour
    {
        public event Action LoadDone;

        [SerializeField]
        ImageTargetCreator imageTargetCreator;
        [SerializeField]
        GameObject modelContainer;

        [SerializeField]
        Modal modalPrefab;
        [SerializeField]
        GameObject canvas;
        [SerializeField]
        ProgressBar progressBar;
        [SerializeField]
        GameObject loadingPanel;

        void Start()
        {
            Load(ARSceneParam.assetInformation);
        }


        private void Load(AssetInformation assetInformation)
        {
            progressBar.EnableProgressBar();
            imageTargetCreator.CreateImageTarget(assetInformation.markerImage, assetInformation.markerImageWidth);
            imageTargetCreator.CreateTargetDoneEvent += () =>
            {
                LoadAsset(assetInformation);
            };
            imageTargetCreator.CreateTargetErrorEvent += (string error) =>
            {
                if (VuforiaApplication.Instance.IsInitialized)
                {
                    TestForARCoreSupportBeforeLoadAsset(assetInformation);
                }
                else
                {
                    VuforiaApplication.Instance.OnVuforiaInitialized += (VuforiaInitError err) =>
                    {
                        TestForARCoreSupportBeforeLoadAsset(assetInformation);
                    };
                }
            };
            imageTargetCreator.OnProgressChanged += (value, text) =>
            {
                progressBar.SetProgress(value, text);
            };
        }

        private void TestForARCoreSupportBeforeLoadAsset(AssetInformation assetInformation)
        {
            if (VuforiaBehaviour.Instance.World.AnchorsSupported)
            {
                LoadAsset(assetInformation);
            } else
            {
                progressBar.DisableProgressBar();
            }
        }

        private void LoadAsset(AssetInformation assetInformation)
        {
            AssetContainer.Instance.LoadAssets(assetInformation.assets, () =>
            {
                progressBar.DisableProgressBar();
                LoadMetadata(assetInformation);
            }, (error) => {
                Modal modal = Instantiate(modalPrefab, canvas.transform);
                modal.SetModalContent("Error", error);
                modal.DisableCancelButton();
                progressBar.DisableProgressBar();
            }, progressBar.SetProgress);
        }

        private void LoadMetadata(AssetInformation assetInformation)
        {
            MetadataObject metadataObject = JsonUtility.FromJson<MetadataObject>(assetInformation.metadata);
            if (metadataObject == null)
            {
                EntityContainer.Instance.InitMetadata(assetInformation.assets);
            }
            else
            {
                EntityContainer.Instance.ApplyMetadata(metadataObject);
            }
            LoadDone?.Invoke();
            loadingPanel.SetActive(false);
        }
    }
}

