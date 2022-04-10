using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
		
		public static string GetFileSizeString(long size)
        {
            if (size < 1000)
            {
                return size + " b";
            }
            else if (size < 1000000)
            {
                return ((float)size / 1000).ToString("#.#") + " kb";
            }
            else
            {
                return ((float)size / 1000000).ToString("#.#") + " mb";
            }
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public Sprite Texture2DToSprite(Texture2D texture2D)
        {
            return Sprite.Create(texture2D,
                new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f)
                );
        }

        public void GetSound(string soundUrl, string extension, Action<AudioClip> callback, Action<string> errorCallback = null, Action<float> progressCallback = null)
        {
            StartCoroutine(GetSoundCoroutine(soundUrl, extension, callback, errorCallback, progressCallback));
        } 

        public void GetImageAsTexture2D(string imageUrl, Action<Texture2D> callback, Action<string> errorCallback = null, Action<float> progressCallback = null)
        {
            StartCoroutine(GetImageCoroutine(imageUrl, callback, errorCallback, progressCallback));
        }

        public static Bounds GetUIBounds(GameObject UIObject)
        {
            Image[] images = UIObject.GetComponentsInChildren<Image>();
            if (images.Length == 0)
            {
                return new Bounds();
            }
            var min = Vector3.positiveInfinity;
            var max = Vector3.negativeInfinity;

            foreach (var image in images)
            {
                if (!image) continue;

                // Get the 4 corners in world coordinates
                var v = new Vector3[4];
                image.rectTransform.GetWorldCorners(v);

                // update min and max
                foreach (var vector3 in v)
                {
                    min = Vector3.Min(min, vector3);
                    max = Vector3.Max(max, vector3);
                }
            }

            // create the bounds
            var bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
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

        private IEnumerator GetSoundCoroutine(string soundUrl, string extension, Action<AudioClip> callback, Action<string> errorCallback, Action<float> progressCallback)
        {
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(soundUrl, GetAudioTypeFromExtension(extension)))
            {
                UnityWebRequestAsyncOperation operation = uwr.SendWebRequest();
                while (!operation.isDone) {
                    progressCallback?.Invoke(operation.progress);
                    yield return null;
                }
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    errorCallback?.Invoke(uwr.error);
                }
                else
                {
                    AudioClip audioClip = DownloadHandlerAudioClip.GetContent(uwr);
                    callback?.Invoke(audioClip);
                }
            }
        }

        private AudioType GetAudioTypeFromExtension(string extension)
        {
            switch (extension)
            {
                case "wav":
                    return AudioType.WAV;
                default:
                    return AudioType.MPEG;
            }
        }

        private IEnumerator GetImageCoroutine(string imageUrl, Action<Texture2D> callback, Action<string> errorCallback, Action<float> progressCallback)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                UnityWebRequestAsyncOperation operation = uwr.SendWebRequest();
                while (!operation.isDone)
                {
                    progressCallback?.Invoke(operation.progress);
                    yield return null;
                }

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    errorCallback?.Invoke(uwr.error);
                }
                else
                {
                    // Get downloaded texture once the web request completes
                    Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                    // https://answers.unity.com/questions/391612/any-way-to-validate-wwwtexture.html
                    if (texture.width < 10 && texture.height < 10)
                    {
                        errorCallback?.Invoke("Error loading texture");
                        yield break;
                    }
                    callback?.Invoke(texture);
                }
            }
        }
    }
}

