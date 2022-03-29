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
        public event Action OnDownloadedButtonClick;
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
        private Button downloadedButton;
        [SerializeField]
        private Button scanQRCodeButton;
        [SerializeField]
        private Toggle homeToggle;
        [SerializeField]
        private Toggle courseToggle;
        [SerializeField]
        private Toggle settingToggle;

        void Awake()
        {
            logoutButton.onClick.AddListener(() =>
            {
                OnLogoutButtonClick?.Invoke();
            });
            downloadedButton.onClick.AddListener(() =>
            {
                OnDownloadedButtonClick?.Invoke();
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
        }

        void Start()
        {
            homeToggle.isOn = true;
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
            this.nameText.text = name;
            this.usernameText.text = username;
        }

        public void PopulateUserAvatar(Sprite avatar)
        {
            userAvatar.sprite = avatar;
        }

        public override void Refresh(object args = null)
        {
        }
    }
}
