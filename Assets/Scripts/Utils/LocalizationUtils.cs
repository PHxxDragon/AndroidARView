using UnityEngine.Localization.Settings;
using UnityEngine;

namespace EAR
{
    public class LocalizationUtils : MonoBehaviour
    {
        public static string GetLocalizedText(string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString("UI", key);
        }
    }
}

