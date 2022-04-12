using System.Collections.Generic;
using UnityEngine;
using EAR.Entity;
using System.Linq;
using System;

namespace EAR.Container
{
    public class EntityContainer : MonoBehaviour
    {
        [SerializeField]
        private GameObject container;
        [SerializeField]
        private EnvironmentController environmentController;
        [SerializeField]
        private float scaleToSize = 1f;
        [SerializeField]
        private float distanceToPlane = 0f;

        private static EntityContainer instance;

        public static EntityContainer Instance
        {
            get
            {
                return instance;
            }
        }

        private Dictionary<string, BaseEntity> entityDict = new Dictionary<string, BaseEntity>();

        void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("Two instance of entity container found");
            }

            BaseEntity.OnEntityCreated += EntityCreatedHandler;
            BaseEntity.OnEntityDestroy += EntityDestroyedHandler;
        }

        private void EntityCreatedHandler(BaseEntity entity)
        {
            TransformData.SetParent(entity.transform, container.transform);
            entityDict.Add(entity.GetId(), entity);
        }

        private void EntityDestroyedHandler(BaseEntity entity)
        {
            if (entityDict.ContainsKey(entity.GetId()))
            {
                entityDict.Remove(entity.GetId());
            }
        }


        void OnDestroy()
        {
            BaseEntity.OnEntityCreated -= EntityCreatedHandler;
            BaseEntity.OnEntityDestroy -= EntityDestroyedHandler;
        }

        public void ApplyMetadata(MetadataObject metadataObject)
        {
            if (metadataObject.modelDatas != null)
            {
                foreach (ModelData modelData in metadataObject.modelDatas)
                {
                    ModelEntity.InstantNewEntity(modelData);
                }
            }
            if (metadataObject.noteDatas != null)
            {
                foreach (NoteData noteData in metadataObject.noteDatas)
                {
                    NoteEntity.InstantNewEntity(noteData);
                }
            }
            if (metadataObject.imageDatas != null)
            {
                foreach (ImageData imageData in metadataObject.imageDatas)
                {
                    ImageEntity.InstantNewEntity(imageData);
                }
            }
            if (metadataObject.soundDatas != null)
            {
                foreach (SoundData soundData in metadataObject.soundDatas)
                {
                    SoundEntity.InstantNewEntity(soundData);
                }
            }
            if (metadataObject.buttonDatas != null)
            {
                foreach (ButtonData buttonData in metadataObject.buttonDatas)
                {
                    ButtonEntity.InstantNewEntity(buttonData);
                }
            }

            environmentController.SetAmbientLight(metadataObject.ambientColor);
            if (metadataObject.lightDatas.Count > 0)
            {
                environmentController.SetDirectionalLight(metadataObject.lightDatas[0]);
            }
            else
            {
                environmentController.SetDirectionalLight(new LightData());
            }
        }

        public void InitMetadata(List<AssetObject> assetObjects)
        {
            environmentController.SetAmbientLight(Color.white);
            environmentController.SetDirectionalLight(new LightData());

            foreach (AssetObject assetObject in assetObjects)
            {
                if (assetObject.type == AssetObject.MODEL_TYPE)
                {
                    ModelData modelData = new ModelData();
                    modelData.assetId = assetObject.assetId;
                    ModelEntity modelEntity = ModelEntity.InstantNewEntity(modelData);
                    Bounds bounds = Utils.GetModelBounds(modelEntity.gameObject);
                    float ratio = scaleToSize / bounds.extents.magnitude;
                    modelEntity.transform.position = -(bounds.center * ratio) + new Vector3(0, distanceToPlane + bounds.extents.y * ratio, 0);
                    modelEntity.transform.localScale *= ratio;
                    break;
                }
            }
        }

        public BaseEntity GetEntity(string entityId)
        {
            try
            {
                return entityDict[entityId];
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

        public BaseEntity[] GetEntities()
        {
            return entityDict.Values.ToArray();
        }


    }
}

