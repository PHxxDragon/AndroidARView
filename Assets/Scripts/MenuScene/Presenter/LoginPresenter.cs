using EAR.WebRequest;
using EAR.View;
using UnityEngine;
using EAR.SceneChange;

namespace EAR.MenuScene.Presenter
{
    public class LoginPresenter : MonoBehaviour
    {
        [SerializeField]
        private WebRequestHelper webRequest;

        [SerializeField]
        private LoginView loginView;

        [SerializeField]
        private CourseListView courseListView;

        [SerializeField]
        private ModuleListView moduleListView;

        [SerializeField]
        private ModelDetailView modelDetailView;

        [SerializeField]
        private Sidebar sidebar;

        [SerializeField]
        private ScreenNavigator screenNavigator;

        void Awake()
        {
            MenuSceneParam.OnLogOut += ResetSideBar;
        }

        void OnDestroy()
        {
            MenuSceneParam.OnLogOut -= ResetSideBar;
        }

        private void ResetSideBar()
        {
            sidebar.ResetUserAvatar();
        }

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
            sidebar.PopulateUserDetail(userProfileData.name, userProfileData.email);
            sidebar.Refresh();
            Utils.Instance.GetImageAsTexture2D(userProfileData.avatar, LoadAvatarSucceedCallback);
            OpenLastView();
        }

        private void OpenLastView()
        {
            if (MenuSceneParam.courseId != -1)
            {
                screenNavigator.OpenView(moduleListView);
                moduleListView.Refresh((MenuSceneParam.courseId, MenuSceneParam.courseName));
            } 
            else if (MenuSceneParam.modelId != -1)
            {
                screenNavigator.OpenView(modelDetailView);
                modelDetailView.Refresh(MenuSceneParam.modelId);
            }
            else
            {
                screenNavigator.OpenView(courseListView);
                courseListView.Refresh();
            }
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

