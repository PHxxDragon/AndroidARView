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
        private const string CATEGORY = "Category";

        public event Action<int> OnLoadModel;

        [SerializeField]
        private Image coverImage;
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
        private ContentSizeFitter contentSizeFitter;

        private UnityAction listener;
        private Sprite originalCoverImage;

        public override void Refresh(object args = null)
        {
            int id = (int)args;
            OnLoadModel?.Invoke(id);
        }

        void Start()
        {
            originalCoverImage = coverImage.sprite;
        }

        public void Clear()
        {
            coverImage.sprite = originalCoverImage;
            title.text = "";
            downloadSize.text = "";
            likes.text = "";
            description.text = "";
            categories.text = "";
            if (listener != null)
            {
                viewInARButton.onClick.RemoveListener(listener);
                listener = null;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentSizeFitter.gameObject.transform as RectTransform);
        }

        public void PopulateData(ModelDataObject modelDataObject)
        {
            modelDataObject.coverImage += (image) =>
            {
                coverImage.sprite = image;
            };
            title.text = modelDataObject.name;
            downloadSize.text = LocalizationUtils.GetLocalizedText(DOWNLOAD_SIZE) + Utils.GetFileSizeString(modelDataObject.totalSize);
            likes.text = modelDataObject.numOfFav + LocalizationUtils.GetLocalizedText(LIKES);
            description.text = modelDataObject.description;
            categories.text = modelDataObject.categories.Count == 0 ? LocalizationUtils.GetLocalizedText(NO_CATEGORY) : LocalizationUtils.GetLocalizedText(CATEGORY) + string.Join(", ", modelDataObject.categories);
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
    }

}
