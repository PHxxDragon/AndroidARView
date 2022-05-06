using EAR.View;
using UnityEngine;
using System.Collections.Generic;
using EAR.AssetCache;
using System.Linq;
using System.Collections;

namespace EAR.Presenter
{
    public class SettingPresenter : MonoBehaviour
    {
        [SerializeField]
        private SettingView settingView;
        [SerializeField]
        private SelectModal selectModalPrefab;
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private AssetCacher assetCacher;

        private const string CHOOSE_LANGUAGE = "ChooseLanguage";
        private const string CHOOSE_MAX_STORAGE = "ChooseMaxStorage";
        private const string ENGLISH = "English";
        private const string TIENG_VIET = "TiengViet";
        private const string CLEAR_CACHE = "ClearCache";
        private const string CLEAR = "Clear";
        private const long Megabyte = 1024 * 1024;

        private readonly List<string> languages = new List<string>()
        {
            "en", "vi"
        };

        private readonly List<string> names = new List<string>();

        private readonly List<long> avaliableStorageSelection = new List<long>()
        {
            64 * Megabyte, 128 * Megabyte, 256 * Megabyte, 512 * Megabyte, 1024 * Megabyte, 2048 * Megabyte
        };

        private List<string> availableStorageObj;
        private List<string> availableStorageName;

        private List<string> clearCacheListName = new List<string>();

        void Awake()
        {
            settingView.OnLanguageChangeButtonClick += OpenLanguageSetting;
            settingView.OnMaxStorageChangeButtonClick += OpenMaxStorageSetting;
            settingView.OnClearStorageButtonClick += OpenClearStorageSetting;
            settingView.OnRefresh += Refresh;
            availableStorageObj = avaliableStorageSelection.Select((value) => value.ToString()).ToList();
            availableStorageName = avaliableStorageSelection.Select((value) => Utils.GetFileSizeString(value)).ToList();
        }

        void Start()
        {
            StartCoroutine(PopulateLocalizedList());
        }

        private IEnumerator PopulateLocalizedList()
        {
            // prevent an unexpected exception from unity localization
            for (int i = 0; i < 20; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            names.Clear();
            names.Add(LocalizationUtils.GetLocalizedText(ENGLISH));
            names.Add(LocalizationUtils.GetLocalizedText(TIENG_VIET));
            clearCacheListName.Clear();
            clearCacheListName.Add(LocalizationUtils.GetLocalizedText(CLEAR));
        }

        private void Refresh()
        {
            string currentLocaleCode = LocalizationUtils.GetCurrentLanguageCode();
            settingView.SetLanguageUsed(names[languages.IndexOf(currentLocaleCode)]);

            long maxStorage = assetCacher.GetMaxStorage();
            settingView.SetMaxStorage(Utils.GetFileSizeString(maxStorage));

            long currentStorage = assetCacher.GetCurrentStorage();
            settingView.SetStorageUsed(Utils.GetFileSizeString(currentStorage));
        }

        private void OpenLanguageSetting()
        {
            SelectModal selectModal = Instantiate(selectModalPrefab, canvas.transform);
            selectModal.PopulateData(LocalizationUtils.GetLocalizedText(CHOOSE_LANGUAGE), languages, names);
            selectModal.OnOptionSelected += (obj) =>
            {
                LocalizationUtils.SetLocale(obj);
                settingView.SetLanguageUsed(names[languages.IndexOf(obj)]);
                StartCoroutine(PopulateLocalizedList());
            };
        }

        private void OpenMaxStorageSetting()
        {
            SelectModal selectModal = Instantiate(selectModalPrefab, canvas.transform);
            selectModal.PopulateData(LocalizationUtils.GetLocalizedText(CHOOSE_MAX_STORAGE), availableStorageObj, availableStorageName);
            selectModal.OnOptionSelected += (obj) =>
            {
                assetCacher.SetMaxStorage(long.Parse(obj));
                settingView.SetMaxStorage(availableStorageName[availableStorageObj.IndexOf(obj)]);
            };
        }

        private void OpenClearStorageSetting()
        {
            SelectModal selectModal = Instantiate(selectModalPrefab, canvas.transform);
            selectModal.PopulateData(LocalizationUtils.GetLocalizedText(CLEAR_CACHE), clearCacheListName, clearCacheListName);
            selectModal.OnOptionSelected += (obj) =>
            {
                assetCacher.ClearAllCache();
                settingView.SetStorageUsed(Utils.GetFileSizeString(assetCacher.GetCurrentStorage()));
            };
        }
    }
}

