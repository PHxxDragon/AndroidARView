using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    [RequireComponent(typeof(Animator))]
    public class LoginView : ViewInterface
    {
        public event Action<string, string> LoginEvent;

        [SerializeField]
        private TMP_InputField emailInputField;

        [SerializeField]
        private TMP_InputField passwordInputField;

        [SerializeField]
        private Button loginButton;

        [SerializeField]
        private ViewInterface homeScreen;

        [SerializeField]
        private TMP_Text errorText;

        private Animator animator;
        private string transparentClose = "TransparentClosing";
        private string transparentOpen = "TransparentOpening";

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            loginButton.onClick.AddListener(LoginButtonClickEventSubscriber);
        }

        public void SetLoginErrorMessage(string message)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = message;
        }

        public override void OpenView(object args)
        {
            animator.enabled = true;
            animator.Play(transparentOpen);
        }

        public override void CloseView()
        {
            animator.enabled = true;
            animator.Play(transparentClose);
        }

        public void DisableAnimator()
        {
            animator.enabled = false;
        }

        public override void Refresh(object args)
        {
        }

        private void LoginButtonClickEventSubscriber()
        {
            if (errorText.gameObject.activeSelf)
                errorText.gameObject.SetActive(false);

            string email = emailInputField.text;
            string password = passwordInputField.text;
            LoginEvent(email, password);
        }
    }
}