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
        private WorkspaceListView workspaceListView;

        [SerializeField]
        private Sidebar sidebar;

        [SerializeField]
        private ScreenNavigator screenNavigator;

        void Start()
        {
            if (webRequest != null && loginView != null)
            {
                loginView.LoginEvent += LoginEventSubscriber;
            }
            else
            {
                Debug.LogWarning("Unassigned references");
            }
        }

        private void LoginEventSubscriber(string username, string password)
        {
            webRequest.Login(username, password, LoginSuccessCallback, LoginErrorCallback);
        }

        private void LoginSuccessCallback()
        {
            screenNavigator.OpenView(NavigateCommandEnum.ToWorkspaceList);

            if (workspaceListView != null && sidebar != null)
            {
                workspaceListView.Refresh();
                sidebar.Refresh();
            }
            else
            {
                Debug.LogWarning("Unassigned reference sidebar or workspaceListView to Login Presenter");
            }


        }
        private void LoginErrorCallback(string errorMessage)
        {
            loginView.SetLoginErrorMessage(errorMessage);
        }
    }
}

