using UnityEngine;
using UnityEngine.SceneManagement;
using EAR.WebRequest;
using EAR.AR;
using EAR.SceneChange;
using EAR.View;

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

        private MetadataObject metadata;
        void Start()
        {
            if (SceneChangeParam.moduleARInformation != null)
            {
                Debug.Log("extension: " + SceneChangeParam.moduleARInformation.extension);
                Debug.Log("image url: " + SceneChangeParam.moduleARInformation.imageUrl);
                Debug.Log("model url: " + SceneChangeParam.moduleARInformation.modelUrl);
                Debug.Log("iszipfile: " + SceneChangeParam.moduleARInformation.isZipFile);
                LoadModule(SceneChangeParam.moduleARInformation);
            }
            else
            {
                Debug.LogError("Unexpected error happened, cannot find module information");
            }
            /*            ModuleARInformation moduleARInformation = new ModuleARInformation();
                        moduleARInformation.modelUrl = "https://firebasestorage.googleapis.com/v0/b/education-ar-c395d.appspot.com/o/models%2F1%2Fmodels_1_Bodacious%20Maimu-Duup%20(2).zip?alt=media&token=6d0d07bd-75ba-4db8-b344-ed96be01ba63";
                        moduleARInformation.extension = "zip";
                        moduleARInformation.imageUrl = "https://firebasestorage.googleapis.com/v0/b/education-ar-c395d.appspot.com/o/ARModule%2F1200px-Florida_Box_Turtle_Digon3_re-edited.jpg?alt=media&token=f4ac3235-3be7-463f-9de3-aea674feaa94";
                        moduleARInformation.metadataString = "{\"modelTransform\":{\"position\":{\"x\":-0.0003384668380022049,\"y\":0.09364131093025208,\"z\":0.48236414790153506},\"rotation\":{\"x\":-0.6999889612197876,\"y\":0.0,\"z\":0.0,\"w\":0.7141537666320801},\"scale\":{\"x\":0.02199966087937355,\"y\":0.02199966087937355,\"z\":0.02199966087937355}},\"imageWidthInMeters\":1.0}";
                        LoadModule(moduleARInformation);*/
        }

        private void LoadModule(ModuleARInformation moduleAR)
        {
            modelLoader.LoadModel(moduleAR.modelUrl, moduleAR.name, moduleAR.id, moduleAR.extension, moduleAR.isZipFile);
            modelLoader.OnLoadEnded += SetModelAsContainerChild;
            modelLoader.OnLoadError += ShowError;
            MetadataObject metadataObject = JsonUtility.FromJson<MetadataObject>(moduleAR.metadataString);
            if (metadataObject == null)
            {
                imageTargetCreator.CreateImageTarget(moduleAR.imageUrl);
                LoadWithoutMetadata();
            } else
            {
                imageTargetCreator.CreateImageTarget(moduleAR.imageUrl, metadataObject.imageWidthInMeters);
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
        }

        private void LoadWithoutMetadata()
        {
            Debug.Log("Load model without metadata");
            modelLoader.OnLoadEnded += ApplyDefaultToModel;
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

