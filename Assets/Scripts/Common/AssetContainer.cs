using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using EAR.Entity;
using EAR.AR;
using TMPro;

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
        private readonly Dictionary<string, (AssetObject, string)> videos = new Dictionary<string, (AssetObject, string)>();
        private readonly Dictionary<string, (AssetObject, TMP_FontAsset)> fonts = new Dictionary<string, (AssetObject, TMP_FontAsset)>();

        [SerializeField]
        private ModelLoader modelLoader;

        [SerializeField]
        private GameObject disabledContainer;

        [SerializeField]
        private GameObject modelPrefab;

        [SerializeField]
        private NoteEntity notePrefab;

        [SerializeField]
        private VideoEntity videoPrefab;

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

        private string GetTempSaveFolder(string folderName)
        {
            return Path.Combine(Application.persistentDataPath, "tempAssets", folderName);
        }

        void OnDestroy()
        {
            if (Directory.Exists(GetTempSaveFolder("")))
            {
                Directory.Delete(GetTempSaveFolder(""), true);
            }
        }

        public void LoadAssets(List<AssetObject> assetObjects, Action callback = null, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            assetCount = assetObjects.Count;
            hasError = false;
            foreach (AssetObject assetObject in assetObjects)
            {
                switch (assetObject.type)
                {
                    case AssetObject.MODEL_TYPE:
                        LoadModel(assetObject, errorCallback, progressCallback);
                        break;
                    case AssetObject.IMAGE_TYPE:
                        LoadImage(assetObject, errorCallback, progressCallback);
                        break;
                    case AssetObject.SOUND_TYPE:
                        LoadSound(assetObject, errorCallback, progressCallback);
                        break;
                    case AssetObject.VIDEO_TYPE:
                        LoadVideo(assetObject, errorCallback, progressCallback);
                        break;
                    case AssetObject.FONT_TYPE:
                        LoadFont(assetObject, errorCallback, progressCallback);
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

        private void LoadModel(AssetObject assetObject, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            modelLoader.LoadModel(assetObject.url, assetObject.extension, assetObject.isZipFile,
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

        private void LoadSound(AssetObject assetObject, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            Utils.Instance.GetSound(assetObject.url, assetObject.extension,
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
                    //TODO: label
                    progressCallback?.Invoke(value, "Loading... " + string.Format("{0:0.00}%", value * 100));
                }
            });
        }

        private void LoadImage(AssetObject assetObject, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            Utils.Instance.GetImageAsTexture2D(assetObject.url,
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
            , (value) => 
            {
                if (!hasError)
                {
                    //TODO: label
                    progressCallback?.Invoke(value, "Loading... " + string.Format("{0:0.00}%", value * 100));
                }
            });
        }

        private void LoadVideo(AssetObject assetObject, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            if (!assetObject.predownload)
            {
                AddVideo(assetObject, assetObject.url);
                assetCount -= 1;
            }
            else
            {
                Utils.Instance.GetFile(assetObject.url, assetObject.extension, GetTempSaveFolder("videos"), (url) =>
                {
                    AddVideo(assetObject, new Uri(url).AbsoluteUri);
                    assetCount -= 1;
                }, 
                (error) => 
                {
                    if (!hasError)
                    {
                        hasError = true;
                        errorCallback?.Invoke(error + " asset name: " + assetObject.name);
                    }
                }, 
                (value) =>
                {
                    if (!hasError)
                    {
                        //TODO: label
                        progressCallback?.Invoke(value, "Loading... " + string.Format("{0:0.00}%", value * 100));
                    }
                });
            }
        }

        private void LoadFont(AssetObject assetObject, Action<string> errorCallback = null, Action<float, string> progressCallback = null)
        {
            Utils.Instance.GetFile(assetObject.url, assetObject.extension, GetTempSaveFolder("fonts"), (url) =>
            {
                AddFont(assetObject, url);
                assetCount -= 1;
            },
               (error) =>
               {
                   if (!hasError)
                   {
                       hasError = true;
                       errorCallback?.Invoke(error + " asset name: " + assetObject.name);
                   }
               },
               (value) =>
               {
                   if (!hasError)
                   {
                       //TODO: label
                       progressCallback?.Invoke(value, "Loading... " + string.Format("{0:0.00}%", value * 100));
                   }
               });
        }

        public TMP_FontAsset GetFont(string assetId)
        {
            try
            {
                return fonts[assetId].Item2;
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

        public string GetVideo(string assetId)
        {
            try
            {
                return videos[assetId].Item2;
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

        private void AddVideo(AssetObject assetObject, string url)
        {
            videos.Add(assetObject.assetId, (assetObject, url));
            OnAssetObjectAdded?.Invoke(assetObject);
        }

        private void AddFont(AssetObject assetObject, string url)
        {
           
            if (File.Exists(url))
            {
                Font font = new Font(url);
                TMP_FontAsset tMP_FontAsset = TMP_FontAsset.CreateFontAsset(font);
                fonts.Add(assetObject.assetId, (assetObject, tMP_FontAsset));
                OnAssetObjectAdded?.Invoke(assetObject);
            } else
            {
                Debug.LogError("Cannot read file " + url);
            }
            
        }

        private void AddModel(AssetObject assetObject, GameObject model)
        {
            OnAssetObjectAdded?.Invoke(assetObject);
            models.Add(assetObject.assetId, (assetObject, model));
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

        public VideoEntity GetVideoPrefab()
        {
            return videoPrefab;
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

