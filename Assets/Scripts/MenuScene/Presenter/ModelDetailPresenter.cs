using UnityEngine;
using EAR.WebRequest;
using EAR.SceneChange;
using UnityEngine.SceneManagement;

namespace EAR.View
{
    public class ModelDetailPresenter : MonoBehaviour
    {
        private const string ERROR = "Error";

        [SerializeField]
        private ModelDetailView modelDetailView;
        [SerializeField]
        private WebRequestHelper webRequestHelper;

        [SerializeField]
        private Modal modalPrefab;
        [SerializeField]
        private Transform canvas;

        void Awake()
        {
            modelDetailView.OnLoadModel += (int id) =>
            {
                webRequestHelper.GetModelDetail(id,
                    (response) =>
                    {
                        for (int i = 0; i < response.categories.Count; i++)
                        {
                            response.categories[i] = webRequestHelper.GetLocalizedCategory(int.Parse(response.categories[i]));
                        }
                        modelDetailView.PopulateData(response);
                        if (response.images.Count > 0)
                        {
                            Utils.Instance.GetImageAsTexture2D(response.images[0], (image) =>
                            {
                                response.coverImage?.Invoke(Utils.Instance.Texture2DToSprite(image));
                            }, (error) =>
                            {
                                Debug.Log(error);
                            });
                        }
                        
                        response.onClick += () =>
                        {
                            webRequestHelper.GetModelARData(id,
                            (arData) =>
                            {
                                ModuleARInformation moduleARInformation = new ModuleARInformation();
                                moduleARInformation.id = id;
                                moduleARInformation.imageUrl = arData.markerImage;
                                moduleARInformation.markerImageWidth = arData.markerImageWidth;
                                moduleARInformation.isZipFile = response.isZipFile;
                                moduleARInformation.extension = response.extension;
                                moduleARInformation.name = response.name;
                                moduleARInformation.modelUrl = response.url;
                                moduleARInformation.metadataString = arData.metadata;
                                ARSceneParam.moduleARInformation = moduleARInformation;
                                SceneManager.LoadScene("ARScene");
                            }, (error) =>
                            {
                                ShowError(error);
                            });
                        };
                    }, 
                    (error) =>
                    {
                        ShowError(error);
                    });
            };
        }

        private void ShowError(string error)
        {
            Modal modal = Instantiate<Modal>(modalPrefab, canvas);
            modal.SetModalContent(Utils.GetLocalizedText(ERROR), error);
            modal.DisableCancelButton();
            modal.OnConfirmButtonClick += () =>
            {
                Destroy(modal.gameObject);
            };
        }
    }
}

