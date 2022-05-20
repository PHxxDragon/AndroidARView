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

        [SerializeField]
        private CourseListView courseList;
        [SerializeField]
        private ModelListView home;
        [SerializeField]
        private SettingView settingView;

        void Start()
        {
            sidebar.OnLogoutButtonClick += () =>
            {
                sidebar.CloseSidebar();
                screenNavigator.OpenView(loginView);
                webRequestHelper.Logout();
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
            sidebar.OnSidebarToggleChange += (value) =>
            {
                if (!screenNavigator.IsLoggedIn() || screenNavigator.CanGoBack())
                    return;
                switch(value)
                {
                    case Sidebar.SidebarToggle.Home:
                        screenNavigator.OpenView(home);
                        home.Refresh();
                        break;
                    case Sidebar.SidebarToggle.Courses:
                        screenNavigator.OpenView(courseList);
                        courseList.Refresh();
                        break;
                    case Sidebar.SidebarToggle.Settings:
                        screenNavigator.OpenView(settingView);
                        settingView.Refresh();
                        break;
                }
                sidebar.CloseSidebar();
            };
        }
    }
}

