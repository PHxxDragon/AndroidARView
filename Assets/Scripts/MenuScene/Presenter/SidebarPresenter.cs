using UnityEngine;
using EAR.View;
using EAR.WebRequest;

namespace EAR.MenuScene.Presenter
{
    public class SidebarPresenter : MonoBehaviour
    {
        [SerializeField]
        private Sidebar sidebar;

        [SerializeField]
        private WebRequestHelper webRequest;

        void Start()
        {
            if (sidebar != null && webRequest != null)
            {
                sidebar.SidebarRefreshEvent += SidebarRefreshEventSubscriber;
            }
            else
            {
                Debug.LogWarning("Unassigned reference");
            }
        }

        private void SidebarRefreshEventSubscriber()
        {
            string token = webRequest.GetAuthorizeToken();
            OnLoadProfile(token);
        }

        private void OnLoadProfile(string token)
        {
            webRequest.GetProfile(token, GetProfileSuccessCallback, null);
        }

        private void GetProfileSuccessCallback(UserProfileData userProfileData)
        {
            sidebar.PopulateUserDetail(userProfileData.name, userProfileData.email);
            Utils.Instance.GetImageAsTexture2D(userProfileData.avatar, LoadAvatarSucceedCallback);
        }

        private void LoadAvatarSucceedCallback(Texture2D texture2D, object param)
        {
            Sprite sprite = Utils.Instance.Texture2DToSprite(texture2D);
            sidebar.PopulateUserAvatar(sprite);
        }
    }
}

