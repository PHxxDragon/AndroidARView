using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

namespace EAR.View
{
    public class ModelDetailView : ViewInterface
{
        private const string DOWNLOAD_SIZE = "DownloadSize";
        private const string LIKES = "Likes";
        private const string NO_CATEGORY = "NoCategory";
        private const string NO_TAG = "NoTag";
        private const string CATEGORY = "Category";
        private const string TAG = "Tag";

        public event Action<int> OnLoadModel;
        public event Action OnGoBack;

        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text downloadSize;
        [SerializeField]
        private TMP_Text likes;
        [SerializeField]
        private Button viewInARButton;
        [SerializeField]
        private TMP_Text description;
        [SerializeField]
        private TMP_Text categories;
        [SerializeField]
        private TMP_Text tags;
        [SerializeField]
        private ImageList imageList;

        [SerializeField]
        private ContentSizeFitter contentSizeFitter;

        [SerializeField]
        private GameObject container;
        [SerializeField]
        private GameObject loadingIndicator;

        private UnityAction listener;
        [SerializeField]
        private Sprite emptyCoverImage;

        public override void Refresh(object args = null)
        {
            int id = (int)args;
            loadingIndicator.gameObject.SetActive(true);
            container.gameObject.SetActive(false);
            OnLoadModel?.Invoke(id);
        }

        void Awake()
        {
            loadingIndicator.gameObject.SetActive(false);
        }

        public void Clear()
        {
            title.text = "";
            downloadSize.text = "";
            likes.text = "";
            description.text = "";
            categories.text = "";
            tags.text = "";
            imageList.SetNumImage(0);
            if (listener != null)
            {
                viewInARButton.onClick.RemoveListener(listener);
                listener = null;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentSizeFitter.gameObject.transform as RectTransform);
        }

        public void PopulateData(ModelDataObject modelDataObject)
        {
            loadingIndicator.gameObject.SetActive(false);
            container.gameObject.SetActive(true);

            imageList.SetNumImage(modelDataObject.coverImages.Count);
            for (int i = 0; i < modelDataObject.coverImages.Count; i++)
            {
                int j = i;
                modelDataObject.coverImages[i] += (image) =>
                {
                    if (image)
                    {
                        imageList.SetImage(image, j);
                    }
                };
            }
            title.text = modelDataObject.name;
            downloadSize.text = LocalizationUtils.GetLocalizedText(DOWNLOAD_SIZE) + Utils.GetFileSizeString(modelDataObject.size);
            likes.text = modelDataObject.numOfFav + LocalizationUtils.GetLocalizedText(LIKES);
            description.text = modelDataObject.description;
            categories.text = modelDataObject.categories.Count == 0 ? LocalizationUtils.GetLocalizedText(NO_CATEGORY) : LocalizationUtils.GetLocalizedText(CATEGORY) + string.Join(", ", modelDataObject.categories);
            tags.text = modelDataObject.tags.Count == 0 ? LocalizationUtils.GetLocalizedText(NO_TAG) : LocalizationUtils.GetLocalizedText(TAG) + string.Join(", ", modelDataObject.tags);
            if (listener != null)
            {
                viewInARButton.onClick.RemoveListener(listener);
            }
            listener = () =>
            {
                modelDataObject.onClick?.Invoke();
            };
            viewInARButton.onClick.AddListener(listener);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentSizeFitter.gameObject.transform as RectTransform);
        }

        public override void GoBack()
        {
            OnGoBack?.Invoke();
        }
    }

}
