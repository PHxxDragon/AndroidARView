using UnityEngine;
using System;
using EAR.View;
using EAR.Container;
using EAR.SceneChange;
using Vuforia;
using EAR.AR;
using EAR.AssetCache;


namespace EAR.Presenter
{
    public class InitializePresenter : MonoBehaviour
    {
        public event Action LoadDone;

        [SerializeField]
        private ImageTargetCreator imageTargetCreator;
        [SerializeField]
        private GameObject modelContainer;

        [SerializeField]
        private Modal modalPrefab;
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private ProgressBar progressBar;
        [SerializeField]
        private GameObject loadingPanel;
        [SerializeField]
        private AssetCacher assetCacher;

        void Start()
        {
            if (ARSceneParam.hasCached)
            {
                Load(ARSceneParam.assetInformation);
            } else
            {
                StartCache(ARSceneParam.assetInformation);
            }
        }

        private void StartCache(AssetInformation assetInformation)
        {
            progressBar.EnableProgressBar();
            assetCacher.ProcessCache(AssetCacher.GetIdFromTypeAndId(assetInformation.id, assetInformation.type), assetInformation,
            (assetInformation) =>
            {
                progressBar.DisableProgressBar();
                Load(assetInformation);
            }, (error) =>
            {
                Modal modal = Instantiate(modalPrefab, canvas.transform);
                modal.SetModalContent("Error", error);
                modal.DisableCancelButton();
                progressBar.DisableProgressBar();
            }, (progress) =>
            {
                //TODO: all label
                progressBar.SetProgress(progress, "downloading");
            });
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

        void OnDestroy()
        {
            GlobalStates.SetIsPlayMode(false);
        }
    }
}

