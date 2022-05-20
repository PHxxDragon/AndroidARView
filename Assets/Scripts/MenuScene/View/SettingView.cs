using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace EAR.View
{
    public class SettingView : ViewInterface
    {
        public event Action OnRefresh;

        public event Action OnLanguageChangeButtonClick;
        public event Action OnMaxStorageChangeButtonClick;
        public event Action OnClearStorageButtonClick;

        [SerializeField]
        private Button languageChangeButton;
        [SerializeField]
        private Button maxStorageChangeButton;
        [SerializeField]
        private Button clearStorageButton;

        [SerializeField]
        private TMP_Text langaugeUsed;
        [SerializeField]
        private TMP_Text maxStorage;
        [SerializeField]
        private TMP_Text storageUsed;
        [SerializeField]
        private ScreenNavigator screenNavigator;
        [SerializeField]
        private CourseListView courseListView;
        

        void Awake()
        {
            languageChangeButton.onClick.AddListener(() =>
            {
                OnLanguageChangeButtonClick?.Invoke();
            });

            maxStorageChangeButton.onClick.AddListener(() =>
            {
                OnMaxStorageChangeButtonClick?.Invoke();
            });

            clearStorageButton.onClick.AddListener(() =>
            {
                OnClearStorageButtonClick?.Invoke();
            });
        }

        public void SetLanguageUsed(string languageText)
        {
            langaugeUsed.text = languageText;
        }

        public void SetMaxStorage(string maxStorageText)
        {
            maxStorage.text = maxStorageText;
        }

        public void SetStorageUsed(string usedStorageText)
        {
            storageUsed.text = usedStorageText;
        }

        public override void Refresh(object args = null)
        {
            OnRefresh?.Invoke();
        }

        public override void GoBack()
        {
            screenNavigator.OpenView(courseListView);
        }
    }
}

