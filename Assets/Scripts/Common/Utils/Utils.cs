using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Localization.Settings;
using System.Collections;
using System;

namespace EAR 
{
    public class Utils : MonoBehaviour
    {
        private static Utils instance;

        public static Utils Instance
        {
            get
            {
                if (!instance)
                {
                    instance = new GameObject("Utils").AddComponent<Utils>();
                }
                return instance;
            }
        }

        public Sprite Texture2DToSprite(Texture2D texture2D)
        {
            return Sprite.Create(texture2D,
                new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f)
                );
        }

        public static string GetLocalizedText(string key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString("UI Text", key);
        }

        public void GetImageAsTexture2D(string imageUrl, Action<Texture2D, object> callback, Action<string, object> errorCallback = null, object param = null)
        {
            StartCoroutine(GetImageCoroutine(imageUrl, callback, errorCallback, param));
        }

        public static Bounds GetModelBounds(GameObject model)
        {
            MeshRenderer[] meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            if (meshRenderers.Length > 0)
            {
                bounds = meshRenderers[0].bounds;
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    bounds.Encapsulate(meshRenderer.bounds);
                }
            }
            SkinnedMeshRenderer[] skinnedMeshRenderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderers.Length > 0)
            {
                if (meshRenderers.Length == 0)
                {
                    bounds = skinnedMeshRenderers[0].sharedMesh.bounds;
                }
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    bounds.Encapsulate(skinnedMeshRenderer.bounds);
                }
            }
            return bounds;
        }

        private IEnumerator GetImageCoroutine(string imageUrl, Action<Texture2D, object> callback, Action<string, object> errorCallback, object param)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                    errorCallback?.Invoke(uwr.error, param);
                }
                else
                {
                    // Get downloaded texture once the web request completes
                    Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                    callback?.Invoke(texture, param);
                }
            }
        }
    }
}

