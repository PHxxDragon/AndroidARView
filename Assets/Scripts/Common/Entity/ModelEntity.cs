using UnityEngine;
using EAR.Container;
using EAR.AnimationPlayer;

namespace EAR.Entity
{
    public class ModelEntity : VisibleEntity
    {
        private static int count = 1;
        private string assetId;
        private int defaultAnimationIndex = 0;

        protected override string GetDefaultName()
        {
            return "New model " + count++; 
        }

        public override void ResetEntityState()
        {
            base.ResetEntityState();
            AnimPlayer animPlayer = GetComponentInChildren<AnimPlayer>();
            if (animPlayer)
            {
                animPlayer.PlayAnimation(0);
            }
        }

        public override void StartDefaultState()
        {
            base.StartDefaultState();
            AnimPlayer animPlayer = GetComponentInChildren<AnimPlayer>();
            if (animPlayer)
            {
                animPlayer.PlayAnimation(defaultAnimationIndex);
            }
        }

        public ModelData GetModelData()
        {
            ModelData modelData = new ModelData();
            modelData.id = GetId();
            modelData.assetId = assetId;
            modelData.name = GetEntityName();
            modelData.transform = TransformData.TransformToTransformData(transform);
            modelData.defaultAnimation = defaultAnimationIndex;
            modelData.isVisible = isVisible;
            return modelData;
        }

        public void SetDefaultAnimation(int index)
        {
            defaultAnimationIndex = index;
        }

        public void PlayAnimation(int index)
        {
            AnimPlayer animPlayer = GetComponentInChildren<AnimPlayer>();
            if (animPlayer)
            {
                if (index < animPlayer.GetAnimationCount())
                {
                    animPlayer.PlayAnimation(index);
                }
            }
        }

        public void SetModel(string assetId)
        {
            if (this.assetId == assetId)
            {
                return;
            }

            this.assetId = assetId;
            TransformData prev = null;
            foreach (Transform child in transform)
            {
                prev = TransformData.TransformToTransformData(child);
                Destroy(child.gameObject);
            }

            GameObject model = AssetContainer.Instance.GetModel(assetId);
            if (!model) {
                model = AssetContainer.Instance.GetModelPrefab();
            }

            GameObject newChild = Instantiate(model);
            TransformData.ResetTransform(newChild.transform);
            TransformData.SetParent(newChild.transform, transform);

            if (prev != null)
            {
                TransformData.TransformDataToTransfrom(prev, newChild.transform);
            }
            OnEntityChanged?.Invoke(this);
        }

        public static ModelEntity InstantNewEntity(ModelData modelData)
        {
            ModelEntity modelEntity = new GameObject().AddComponent<ModelEntity>();
            GameObject model = AssetContainer.Instance.GetModel(modelData.assetId);

            modelEntity.isVisible = modelData.isVisible;

            if (model)
            {
                GameObject child = Instantiate(model);
                modelEntity.assetId = modelData.assetId;
                TransformData.ResetTransform(child.transform);
                TransformData.SetParent(child.transform, modelEntity.transform);
            } else
            {
                model = AssetContainer.Instance.GetModelPrefab();
                GameObject child = Instantiate(model);
                TransformData.ResetTransform(child.transform);
                TransformData.SetParent(child.transform, modelEntity.transform);
            }

            if (!string.IsNullOrEmpty(modelData.name))
            {
                modelEntity.SetEntityName(modelData.name);
            }
            
            if (modelData.transform != null)
            {
                TransformData.TransformDataToTransfrom(modelData.transform, modelEntity.transform);
            }
            
            if (!string.IsNullOrEmpty(modelData.id))
            {
                modelEntity.SetId(modelData.id);
            }

            if (modelData.defaultAnimation > 0)
            {
                modelEntity.defaultAnimationIndex = modelData.defaultAnimation;
            }

            OnEntityCreated?.Invoke(modelEntity);
            return modelEntity;
        }

        public string GetAssetId()
        {
            return assetId;
        }
    }
}

