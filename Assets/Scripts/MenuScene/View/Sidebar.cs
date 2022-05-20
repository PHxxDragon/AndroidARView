using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

namespace EAR.View
{
    public class Sidebar : ViewInterface
    {
        public enum SidebarToggle { Home, Courses, Settings }

        public event Action OnLogoutButtonClick;
        public event Action OnScanQRCodeButtonClick;
        public event Action<SidebarToggle> OnSidebarToggleChange;

        [SerializeField]
        private Image userAvatar;
        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private TMP_Text usernameText;
        [SerializeField]
        private RectTransform sidebarRect;
        [SerializeField]
        private Button logoutButton;
        [SerializeField]
        private Button scanQRCodeButton;
        [SerializeField]
        private Toggle courseToggle;
        [SerializeField]
        private Toggle homeToggle;
        [SerializeField]
        private Toggle settingToggle;
        [SerializeField]
        private ScreenNavigator screenNavigator;

        void Awake()
        {
            logoutButton.onClick.AddListener(() =>
            {
                OnLogoutButtonClick?.Invoke();
            });
            scanQRCodeButton.onClick.AddListener(() =>
            {
                OnScanQRCodeButtonClick?.Invoke();
            });
            homeToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnSidebarToggleChange?.Invoke(SidebarToggle.Home);
            });
            courseToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnSidebarToggleChange?.Invoke(SidebarToggle.Courses);
            });
            settingToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn) OnSidebarToggleChange?.Invoke(SidebarToggle.Settings);
            });

            screenNavigator.OnViewChanged += SetToggle;
        }

        private void SetToggle(ViewInterface viewInterface)
        {
            if (viewInterface is CourseListView)
            {
                courseToggle.isOn = true;
                homeToggle.isOn = false;
                settingToggle.isOn = false;
            }
            else if (viewInterface is ModelListView)
            {
                homeToggle.isOn = true;
                courseToggle.isOn = false;
                settingToggle.isOn = false;
            }
            else if (viewInterface is SettingView)
            {
                settingToggle.isOn = true;
                homeToggle.isOn = false;
                courseToggle.isOn = false;
            }
        }

        public void OpenSidebar()
        {
            gameObject.SetActive(true);
            sidebarRect.DOAnchorPosX(0, 0.5f).SetEase(Ease.InQuad);
        }

        public void CloseSidebar()
        {
            sidebarRect.DOAnchorPosX(-620f, 0.5f).SetEase(Ease.OutQuad).onComplete += () =>
            {
                gameObject.SetActive(false);
            };
            
        }

        public void PopulateUserDetail(string name, string username)
        {
            nameText.text = name;
            usernameText.text = username;
        }

        public void PopulateUserAvatar(Sprite avatar)
        {
            userAvatar.sprite = avatar;
        }

        public override void Refresh(object args = null)
        {
        }

        public override void GoBack()
        {
            CloseSidebar();
        }
    }
}
