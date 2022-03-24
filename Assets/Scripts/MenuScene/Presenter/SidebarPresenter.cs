using EAR.View;
using EAR.WebRequest;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EAR.Presenter
{
    public class SidebarPresenter : MonoBehaviour
    {
        [SerializeField]
        private Sidebar sidebar;
        [SerializeField]
        private ScreenNavigator screenNavigator;
        [SerializeField]
        private LoginView loginView;
        [SerializeField]
        private WebRequestHelper webRequestHelper;
        [SerializeField]
        private SwipeDetector swipeDetector;

        void Start()
        {
            sidebar.OnLogoutButtonClick += () =>
            {
                sidebar.CloseSidebar();
                screenNavigator.OpenView(loginView);
                webRequestHelper.Logout();
            };
            sidebar.OnDownloadedButtonClick += () =>
            {
                SceneManager.LoadScene("DownloadedScene");
            };
            sidebar.OnScanQRCodeButtonClick += () =>
            {
                SceneManager.LoadScene("QRCodeScene");
            };
            swipeDetector.OnSwipeRight += () =>
            {
                if (!screenNavigator.CanGoBack() && screenNavigator.IsLoggedIn())
                {
                    sidebar.OpenSidebar();
                }
            };
            swipeDetector.OnSwipeLeft += () =>
            {
                sidebar.CloseSidebar();
            };
        }
    }
}

