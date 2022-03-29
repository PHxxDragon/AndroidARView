using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace EAR.View
{
    public class ModelView : ListItemView<ModelDataObject>
    {
        [SerializeField]
        private TMP_Text modelTitle;
        [SerializeField]
        private TMP_Text modelDescription;
        [SerializeField]
        private Image modelImage;
        [SerializeField]
        private Button modelButton;

        private UnityAction listener;

        public override void PopulateData(ModelDataObject modelDataObject)
        {
            modelTitle.text = modelDataObject.name;
            modelDescription.text = modelDataObject.description;
            modelDataObject.coverImage += (image) =>
            {
                modelImage.sprite = image;
            };
            if (listener != null)
            {
                modelButton.onClick.RemoveListener(listener);
            }
            listener = () =>
            {
                modelDataObject.onClick?.Invoke();
            };
            modelButton.onClick.AddListener(listener);
        }
    }
}

