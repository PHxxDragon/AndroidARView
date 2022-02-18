using UnityEngine;

namespace EAR
{
    public static class LocalStorage
    {
        public static void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public static string Load(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public static void Save(string key, object value)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
        }

        public static T Load<T>(string key)
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        }
    }
}
