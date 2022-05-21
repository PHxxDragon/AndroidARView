using UnityEngine;
using EAR.WebRequest;
using EAR.SceneChange;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace EAR.View
{
    public class ModelDetailPresenter : MonoBehaviour
    {
        [SerializeField]
        private ModelDetailView modelDetailView;
        [SerializeField]
        private WebRequestHelper webRequestHelper;
        [SerializeField]
        private ModalShower modalShower;
        [SerializeField]
        private ModelListView modelListView;
        [SerializeField]
        private ScreenNavigator screenNavigator;

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
                        response.coverImages = new List<System.Action<Sprite>>();
                        for (int i = 0; i < response.images.Count; i++)
                        {
                            response.coverImages.Add(null);
                        }

                        modelDetailView.PopulateData(response);

                        for (int i = 0; i < response.images.Count; i++)
                        {
                            int j = i;
                            Utils.Instance.GetImageAsTexture2D(response.images[i], (image) =>
                            {
                                response.coverImages[j]?.Invoke(Utils.Instance.Texture2DToSprite(image));
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
                                ARSceneParam.assetInformation = arData;
                                MenuSceneParam.modelId = id;
                                MenuSceneParam.courseId = -1;
                                SceneManager.LoadScene("ARScene");
                            }, (error) =>
                            {
                                modalShower.ShowErrorModal(error, () =>
                                {
                                    GoBackToModelList();
                                });
                            });
                        };
                    }, 
                    (error) =>
                    {
                        modalShower.ShowErrorModal(error, () =>
                        {
                            GoBackToModelList();
                        });
                    });
            };

            modelDetailView.OnGoBack += () =>
            {
                GoBackToModelList();
            };
        }

        private void GoBackToModelList()
        {
            screenNavigator.OpenView(modelListView);
            modelListView.Refresh();
        }
    }
}

