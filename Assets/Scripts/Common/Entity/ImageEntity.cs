using System;
using UnityEngine;
using UnityEngine.UI;
using EAR.Container;

namespace EAR.Entity
{
    public class ImageEntity : VisibleEntity
    {
        private static int count = 1;
        [SerializeField]
        private Image image;

        private string assetId;

        protected override string GetDefaultName()
        {
            return "New image " + count++;
        }

        public ImageData GetImageData()
        {
            ImageData imageData = new ImageData();
            imageData.assetId = assetId;
            imageData.id = GetId();
            imageData.name = GetEntityName();
            imageData.transform = TransformData.TransformToTransformData(transform);
            imageData.isVisible = isVisible;
            return imageData;
        }

        public void SetImage(string assetId)
        {
            Texture2D image = AssetContainer.Instance.GetImage(assetId); 
            if (!image)
            {
                image = AssetContainer.Instance.GetDefaultImage();
            }
            this.image.sprite = Utils.Instance.Texture2DToSprite(image);
            this.assetId = assetId;
            //OnEntityChanged?.Invoke(this);
        }

        public static ImageEntity InstantNewEntity(ImageData imageData)
        {
            ImageEntity imagePrefab = AssetContainer.Instance.GetImagePrefab();
            ImageEntity imageEntity = Instantiate(imagePrefab);

            if (!string.IsNullOrEmpty(imageData.id))
            {
                imageEntity.SetId(imageData.id);
            }

            Texture2D image = AssetContainer.Instance.GetImage(imageData.assetId);
            if (image)
            {
                imageEntity.image.sprite = Utils.Instance.Texture2DToSprite(image);
                imageEntity.assetId = imageData.assetId;
            } else
            {
                image = AssetContainer.Instance.GetDefaultImage();
                imageEntity.image.sprite = Utils.Instance.Texture2DToSprite(image);
            }

            if (!string.IsNullOrEmpty(imageData.name))
            {
                imageEntity.SetEntityName(imageData.name);
            }

            if (imageData.transform != null)
            {
                TransformData.TransformDataToTransfrom(imageData.transform, imageEntity.transform);
            }

            OnEntityCreated?.Invoke(imageEntity);
            return imageEntity;
        }


        public string GetAssetId()
        {
            return assetId;
        }
    }
}

