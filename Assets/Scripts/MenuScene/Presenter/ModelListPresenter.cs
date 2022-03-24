using EAR.View;
using UnityEngine;
using EAR.WebRequest;

namespace EAR.Presenter
{
    public class ModelListPresenter : MonoBehaviour
    {
        private const string ERROR = "Error";

        [SerializeField]
        private ModelListView modelListView;
        [SerializeField]
        private WebRequestHelper webRequest;
        [SerializeField]
        private ModelDetailView modelDetailView;
        [SerializeField]
        private ScreenNavigator screenNavigator;

        [SerializeField]
        private Modal modalPrefab;
        [SerializeField]
        private Transform canvas;

        void Awake()
        {
            modelListView.ModelListRefreshEvent += (page, limit, modelType) =>
            {
                webRequest.GetModelList(page, limit, modelType == ModelListView.ModelType.Bought,
                    (result) => {
                        foreach (ModelDataObject modelDataObject in result.models)
                        {
                            if (modelDataObject.images.Count > 0)
                            {
                                Utils.Instance.GetImageAsTexture2D(modelDataObject.images[0], (image) =>
                                {
                                    modelDataObject.coverImage?.Invoke(Utils.Instance.Texture2DToSprite(image));
                                });
                            }
                            modelDataObject.onClick += () =>
                            {
                                screenNavigator.PushView(modelDetailView);
                                modelDetailView.Clear();
                                modelDetailView.Refresh(modelDataObject.id);
                            };
                        }
                        modelListView.PopulateData(result.models, result.pageCount);
                    },
                    (error) => {
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

