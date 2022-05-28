using EAR.View;
using UnityEngine;
using EAR.WebRequest;
using EAR.SceneChange;
using System.Collections.Generic;

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

        private int currentPage = -1;
        private int currentLimit = -1;
        private ModelListView.ModelType currentType = ModelListView.ModelType.All;
        private string currentKeyword = "";

        void OnDestroy()
        {
            MenuSceneParam.OnLogOut -= ResetState;
        }

        private void ResetState()
        {
            currentPage = -1;
            currentLimit = -1;
            currentType = ModelListView.ModelType.All;
            currentKeyword = "";
        }

        void Awake()
        {
            MenuSceneParam.OnLogOut += ResetState;

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
                            modelDataObject.coverImages = new List<System.Action<Sprite>>();
                            modelDataObject.coverImages.Add(null);
                            if (modelDataObject.images.Count > 0)
                            {
                                Utils.Instance.GetImageAsTexture2D(modelDataObject.images[0], (image) =>
                                {
                                    modelDataObject.coverImages[0]?.Invoke(Utils.Instance.Texture2DToSprite(image));
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

