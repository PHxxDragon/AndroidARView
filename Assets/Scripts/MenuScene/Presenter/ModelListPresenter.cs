using EAR.View;
using UnityEngine;
using EAR.WebRequest;
using EAR.SceneChange;

namespace EAR.Presenter
{
    public class ModelListPresenter : MonoBehaviour
    {
        [SerializeField]
        private ModelListView modelListView;
        [SerializeField]
        private WebRequestHelper webRequest;
        [SerializeField]
        private ModelDetailView modelDetailView;
        [SerializeField]
        private ScreenNavigator screenNavigator;
        [SerializeField]
        private CourseListView courseListView;
        [SerializeField]
        private ModalShower modalShower;

        private int currentPage;
        private int currentLimit;
        private ModelListView.ModelType currentType;
        private string currentKeyword;

        void Awake()
        {
            modelListView.ModelListRefreshEvent += (page, limit, modelType, keyword) =>
            {
                if (CheckCache(page, limit, modelType, keyword))
                {
                    modelListView.KeepData();
                    return;
                }
                MenuSceneParam.ResetId();
                webRequest.GetModelList(page, limit, modelType.ToString(), keyword,
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
                                screenNavigator.OpenView(modelDetailView);
                                modelDetailView.Clear();
                                modelDetailView.Refresh(modelDataObject.id);
                            };
                        }
                        modelListView.PopulateData(result.models, result.pageCount);
                        ApplyCache(page, limit, modelType, keyword);
                    },
                    (error) => {
                        modalShower.ShowErrorModal(error);
                    });
            };

            modelListView.OnGoBack += () =>
            {
                screenNavigator.OpenView(courseListView);
            };
        }

        private bool CheckCache(int page, int limit, ModelListView.ModelType type, string keyword)
        {
            if (page == currentPage && limit == currentLimit && type == currentType && keyword == currentKeyword)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void ApplyCache(int page, int limit, ModelListView.ModelType type, string keyword)
        {
            currentPage = page;
            currentLimit = limit;
            currentType = type;
            currentKeyword = keyword;
        }
    }
}

