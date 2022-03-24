using EAR.WebRequest;
using EAR.View;
using UnityEngine;

namespace EAR.MenuScene.Presenter
{
    public class LoginPresenter : MonoBehaviour
    {
        [SerializeField]
        private WebRequestHelper webRequest;

        [SerializeField]
        private LoginView loginView;

        [SerializeField]
        private ModelListView modelListView;

        [SerializeField]
        private Sidebar sidebar;

        [SerializeField]
        private ScreenNavigator screenNavigator;

        void Start()
        {
            loginView.LoginEvent += LoginEventSubscriber;
            webRequest.Login(LoginSuccessCallback, (error) => {
                screenNavigator.OpenView(loginView);
            });
        }

        private void LoginEventSubscriber(string username, string password)
        {
            webRequest.Login(username, password, LoginSuccessCallback, LoginErrorCallback);
        }

        private void LoginSuccessCallback(UserProfileData userProfileData)
        {
            screenNavigator.OpenView(modelListView);
            modelListView.Refresh();
            sidebar.PopulateUserDetail(userProfileData.name, userProfileData.email);
            Utils.Instance.GetImageAsTexture2D(userProfileData.avatar, LoadAvatarSucceedCallback);
        }

        private void LoginErrorCallback(string errorMessage)
        {
            loginView.SetLoginErrorMessage(errorMessage);
        }

        private void LoadAvatarSucceedCallback(Texture2D texture2D)
        {
            Sprite sprite = Utils.Instance.Texture2DToSprite(texture2D);
            sidebar.PopulateUserAvatar(sprite);
        }
    }
}

