using UnityEngine;
using EAR.WebRequest;
using EAR.SceneChange;
using UnityEngine.SceneManagement;

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
                                ARSceneParam.assetInformation = arData;
                                SceneManager.LoadScene("ARScene");
                            }, (error) =>
                            {
                                modalShower.ShowErrorModal(error);
                            });
                        };
                    }, 
                    (error) =>
                    {
                        modalShower.ShowErrorModal(error);
                    });
            };

            modelDetailView.OnGoBack += () =>
            {
                screenNavigator.OpenView(modelListView);
            };
        }
    }
}

