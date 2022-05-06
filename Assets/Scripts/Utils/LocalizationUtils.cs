using UnityEngine.Localization.Settings;
using UnityEngine;
using UnityEngine.Localization;

namespace EAR
{
    public class LocalizationUtils : MonoBehaviour
    {
        public static string GetLocalizedText(string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString("UI", key);
        }

        public static string GetCurrentLanguageCode()
        {
            return LocalizationSettings.SelectedLocale.Identifier.Code;
        }

        public static void SetLocale(string code)
        {
            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                if (locale.Identifier.Code == code)
                {
                    LocalizationSettings.SelectedLocale = locale;
                    break;
                }
            }
        }
    }
}

