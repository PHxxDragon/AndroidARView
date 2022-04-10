using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using EAR.Entity;
using EAR.AR;

namespace EAR.Container
{
    public class AssetContainer : MonoBehaviour
    {
        private static AssetContainer instance;
        public static event Action<AssetContainer> OnInstanceCreated;

        public static AssetContainer Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {
            if (!instance)
            {
                instance = this;
                OnInstanceCreated?.Invoke(this);
            }
            else
            {
                Debug.LogError("Two instance of asset container found");
            }
        }

        public Action<AssetObject> OnAssetObjectAdded;

        private readonly Dictionary<string, (AssetObject, GameObject)> models = new Dictionary<string, (AssetObject, GameObject)>();
        private readonly Dictionary<string, (AssetObject, Texture2D)> images = new Dictionary<string, (AssetObject, Texture2D)>();
        private readonly Dictionary<string, (AssetObject, AudioClip)> sounds = new Dictionary<string, (AssetObject, AudioClip)>();

        [SerializeField]
        private ModelLoader modelLoader;

        [SerializeField]
        private GameObject disabledContainer;

        [SerializeField]
        private GameObject modelPrefab;

        [SerializeField]
        private NoteEntity notePrefab;

        [SerializeField]
        private Texture2D defaultTexture;

        [SerializeField]
        private ImageEntity imagePrefab;

        [SerializeField]
        private ButtonEntity buttonPrefab;

        [SerializeField]
        private SoundEntity soundPrefab;

        private int assetCount;
        private bool hasError;

        public void LoadAssets(List<AssetObject> assetObjects, Action callback = null, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            assetCount = assetObjects.Count;
            hasError = false;
            foreach (AssetObject assetObject in assetObjects)
            {
                switch (assetObject.type)
                {
                    case AssetObject.MODEL_TYPE:
                        LoadModel(assetObject, assetObject.url, assetObject.extension, assetObject.isZipFile, errorCallback, progressCallback);
                        break;
                    case AssetObject.IMAGE_TYPE:
                        LoadImage(assetObject, assetObject.url, errorCallback, progressCallback);
                        break;
                    case AssetObject.SOUND_TYPE:
                        LoadSound(assetObject, assetObject.url, assetObject.extension, errorCallback, progressCallback);
                        break;
                    default:
                        assetCount -= 1;
                        break;
                }
            }
            StartCoroutine(CheckForLoadAssetDone(callback));
        }

        private IEnumerator CheckForLoadAssetDone(Action callback)
        {
            while (true)
            {
                if (assetCount != 0)
                {
                    yield return new WaitForSecondsRealtime(0.2f);
                } else if (hasError)
                {
                    break;
                }else
                {
                    callback?.Invoke();
                    break;
                }
            }
        }

        private void LoadModel(AssetObject assetObject, string url, string extension, bool isZipFile, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            modelLoader.LoadModel(url, extension, isZipFile,
            (model) =>
            {
                AddModel(assetObject, model);
                assetCount -= 1;
            }, (error) =>
            {
                if (!hasError)
                {
                    hasError = true;
                    errorCallback?.Invoke(error + " asset name: " + assetObject.name);
                }
            }
            , (value, text) => { 
                if (!hasError)
                {
                    progressCallback?.Invoke(value, text);
                }
            });
        }

        private void LoadSound(AssetObject assetObject, string url, string extension, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            Utils.Instance.GetSound(url, extension,
            (audioClip) =>
            {
                AddSound(assetObject, audioClip);
                assetCount -= 1;
            }, (error) =>
            {
                if (!hasError)
                {
                    hasError = true;
                    errorCallback?.Invoke(error + " asset name: " + assetObject.name);
                }
            }
            , (value) => {
                if (!hasError)
                {
                    progressCallback?.Invoke(value, "Loading sound");
                }
            });
        }

        private void LoadImage(AssetObject assetObject, string url, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            Utils.Instance.GetImageAsTexture2D(url,
            (image) =>
            {
                AddImage(assetObject, image);
                assetCount -= 1;
            }, (error) =>
            {
                if (!hasError)
                {
                    hasError = true;
                    errorCallback?.Invoke(error + " asset name: " + assetObject.name);
                }
            }
            , (value) => {
                if (!hasError)
                {
                    progressCallback?.Invoke(value, "Loading image");
                }
            });
        }

        private void AddModel(AssetObject assetObject, GameObject model)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            models.Add(assetObject.assetId, (assetObject, model));
            TransformData.ResetTransform(model.transform);
            TransformData.SetParent(model.transform, disabledContainer.transform);
        }

        public GameObject GetModel(string assetId)
        {
            try
            {
                return models[assetId].Item2;
            } catch (KeyNotFoundException)
            {
                return null;
            } catch (ArgumentNullException)
            {
                return null;
            }
        }

        private void AddImage(AssetObject assetObject, Texture2D image)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            images.Add(assetObject.assetId, (assetObject, image));
        }

        public Texture2D GetImage(string assetId)
        {
            try
            {
                return images[assetId].Item2;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        private void AddSound(AssetObject assetObject, AudioClip sound)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            sounds.Add(assetObject.assetId, (assetObject, sound));
        }

        public AudioClip GetSound(string assetId)
        {
            try
            {
                return sounds[assetId].Item2;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        public GameObject GetModelPrefab()
        {
            return modelPrefab;
        }

        public NoteEntity GetNotePrefab()
        {
            return notePrefab;
        }

        public ImageEntity GetImagePrefab()
        {
            return imagePrefab;
        }

        public Texture2D GetDefaultImage()
        {
            return defaultTexture;
        }

        public ButtonEntity GetButtonPrefab()
        {
            return buttonPrefab;
        }

        public SoundEntity GetSoundPrefab()
        {
            return soundPrefab;
        }
    }
}

