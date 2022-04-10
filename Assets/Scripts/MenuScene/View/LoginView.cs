using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

namespace EAR.View
{
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
        private TMP_Text errorText;

        void Start()
        {
            loginButton.onClick.AddListener(LoginButtonClickEventSubscriber);
        }

        public void SetLoginErrorMessage(string message)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = message;
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

        public void ScanQRCodeClick()
        {
            SceneManager.LoadScene("QRCodeScene");
        }
    }
}