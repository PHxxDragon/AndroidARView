using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    [RequireComponent(typeof(Animator))]
    public class Sidebar : ViewInterface
    {
        public event Action SidebarRefreshEvent;

        [SerializeField]
        private Image userAvatar;
        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private TMP_Text usernameText;

        private Animator sideBarAnimator;
        private string sideBarClose = "SideBarClose";
        private string sideBarOpen = "SideBarOpen";

        public void DisableAnimator()
        {
            sideBarAnimator.enabled = false;
        }

        public void OpenSideBar()
        {
            OpenView(null);
        }

        public override void OpenView(object args = null)
        {
            sideBarAnimator.enabled = true;
            sideBarAnimator.Play(sideBarOpen);
        }

        public override void CloseView()
        {
            sideBarAnimator.enabled = true;
            sideBarAnimator.Play(sideBarClose);
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
            SidebarRefreshEvent?.Invoke();
        }

        private void Awake()
        {
            sideBarAnimator = GetComponent<Animator>();
        }
    }
}
