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

        private string assetId = "";

        public static int GetNextId()
        {
            return count++;
        }

/*        protected override string GetDefaultName()
        {
            return "New image " + count++;
        }*/

        public override EntityData GetData()
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
            if (this.assetId == assetId || assetId == null)
            {
                return;
            }

            this.assetId = assetId;

            Texture2D image = AssetContainer.Instance.GetImage(assetId); 
            if (!image)
            {
                image = AssetContainer.Instance.GetDefaultImage();
            }
            this.image.sprite = Utils.Instance.Texture2DToSprite(image);
        }

        public override void PopulateData(EntityData entityData)
        {
            if (entityData is ImageData imageData)
            {
                base.PopulateData(entityData);

                if (imageData.isVisible.HasValue)
                {
                    isVisible = imageData.isVisible.Value;
                }

                if (imageData.assetId != null)
                {
                    SetImage(imageData.assetId);
                }
            } else
            {
                Debug.LogError("Wrong data class entity id " + entityData.id);
            }
            
        }

        public static ImageEntity InstantNewEntity(ImageData imageData)
        {
            ImageEntity imagePrefab = AssetContainer.Instance.GetImagePrefab();
            ImageEntity imageEntity = Instantiate(imagePrefab);
            imageEntity.PopulateData(imageData);
            OnEntityCreated?.Invoke(imageEntity);
            return imageEntity;
        }


        public string GetAssetId()
        {
            return assetId;
        }
    }
}

