using UnityEngine;
using UnityEngine.SceneManagement;
using EAR.WebRequest;
using EAR.AR;
using EAR.SceneChange;
using EAR.View;
using Vuforia;

namespace EAR.Editor.Presenter
{
    public class ModuleLoadPresenter : MonoBehaviour
    {
        [SerializeField]
        WebRequestHelper webRequestHelper;
        [SerializeField]
        ModelLoader modelLoader;
        [SerializeField]
        ImageTargetCreator imageTargetCreator;
        [SerializeField]
        GameObject modelContainer;
        [SerializeField]
        private float scaleToSize = 0.5f;
        [SerializeField]
        private float distanceToPlane = 0f;
        [SerializeField]
        Modal modalPrefab;
        [SerializeField]
        GameObject canvas;
        [SerializeField]
        Note notePrefab;

        private MetadataObject metadata;
        void Start()
        {
            if (ARSceneParam.moduleARInformation != null)
            {
                Debug.Log("extension: " + ARSceneParam.moduleARInformation.extension);
                Debug.Log("image url: " + ARSceneParam.moduleARInformation.imageUrl);
                Debug.Log("model url: " + ARSceneParam.moduleARInformation.modelUrl);
                Debug.Log("iszipfile: " + ARSceneParam.moduleARInformation.isZipFile);
                LoadImageTarget(ARSceneParam.moduleARInformation);
            }
            else
            {
                Debug.LogError("Unexpected error happened, cannot find module information");
            }
        }

        private void LoadImageTarget(ARInformation moduleAR)
        {
            imageTargetCreator.CreateImageTarget(moduleAR.imageUrl, moduleAR.markerImageWidth);
            imageTargetCreator.CreateTargetDoneEvent += () =>
            {
                LoadModelAndMetadata(moduleAR);
            };
            imageTargetCreator.CreateTargetErrorEvent += (string error) =>
            {
                if (VuforiaApplication.Instance.IsInitialized)
                {
                    TestForARCoreSupportBeforeLoadModel(moduleAR);
                } else
                {
                    VuforiaApplication.Instance.OnVuforiaInitialized += (VuforiaInitError err) =>
                    {
                        TestForARCoreSupportBeforeLoadModel(moduleAR);
                    };
                }
            };
        }

        private void TestForARCoreSupportBeforeLoadModel(ARInformation moduleAR)
        {
            if (VuforiaBehaviour.Instance.World.AnchorsSupported)
            {
                LoadModelAndMetadata(moduleAR);
            }
        }

        private void LoadModelAndMetadata(ARInformation moduleAR)
        {
            modelLoader.LoadModel(moduleAR.modelUrl, moduleAR.name, moduleAR.extension, moduleAR.isZipFile);
            modelLoader.OnLoadEnded += SetModelAsContainerChild;
            modelLoader.OnLoadError += ShowError;
            MetadataObject metadataObject = JsonUtility.FromJson<MetadataObject>(moduleAR.metadataString);
            if (metadataObject == null)
            {
                LoadWithoutMetadata();
            }
            else
            {
                LoadWidthMetadata(metadataObject);
            }
        }

        private void ShowError(string obj)
        {
            Modal modal = Instantiate(modalPrefab, canvas.transform);
            modal.SetModalContent("Error", obj);
            modal.DisableCancelButton();
            modal.OnConfirmButtonClick += GoBackToMenu;
        }

        private void GoBackToMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }

        private void SetModelAsContainerChild()
        {
            modelLoader.GetModel().transform.parent = modelContainer.transform;
        }

        private void LoadWidthMetadata(MetadataObject metadataObject)
        {
            metadata = metadataObject;
            modelLoader.OnLoadEnded += ApplyMetadataToModel;
            foreach (NoteData noteData in metadata.noteDatas)
            {
                Note note = Instantiate(notePrefab, modelContainer.transform);
                note.PopulateData(noteData);
            }
            RenderSettings.ambientLight = metadata.ambientColor;
        }

        private void LoadWithoutMetadata()
        {
            Debug.Log("Load model without metadata");
            modelLoader.OnLoadEnded += ApplyDefaultToModel;
            RenderSettings.ambientLight = Color.white;
        }

        private void ApplyMetadataToModel()
        {
            TransformData.TransformDataToTransfrom(metadata.modelTransform, modelLoader.GetModel().transform);
        }

        private void ApplyDefaultToModel()
        {
            GameObject model = modelLoader.GetModel();
            Bounds bounds = Utils.GetModelBounds(modelLoader.GetModel());
            float ratio = scaleToSize / bounds.extents.magnitude;
            model.transform.localPosition = -(bounds.center * ratio) + new Vector3(0, distanceToPlane + bounds.extents.y * ratio, 0);
            model.transform.localScale *= ratio;
        }
    }
}

