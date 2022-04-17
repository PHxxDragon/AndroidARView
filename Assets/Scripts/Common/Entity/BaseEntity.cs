using UnityEngine;
using System;

namespace EAR.Entity
{
    public class BaseEntity : MonoBehaviour
    {
        public static Action<BaseEntity> OnEntityCreated;
        public static Action<BaseEntity> OnEntityDestroy;

        private string id = Guid.NewGuid().ToString();
        private string entityName;

        private Action<bool> action;

        protected virtual void Awake()
        {
            entityName = GetDefaultName();
            action = (isPlayMode) =>
            {
                if (isPlayMode)
                {
                    StartDefaultState();
                }
                else
                {
                    ResetEntityState();
                }
            };
            GlobalStates.OnIsPlayModeChange += action;

        }

        public virtual EntityData GetData()
        {
            EntityData entityData = new EntityData();
            entityData.id = GetId();
            entityData.name = GetEntityName();
            entityData.transform = TransformData.TransformToTransformData(transform);
            return entityData;
        }

        public virtual void PopulateData(EntityData entityData)
        {
            if (!string.IsNullOrEmpty(entityData.id))
            {
                SetId(entityData.id);
            }
            if (!string.IsNullOrEmpty(entityData.name))
            {
                SetEntityName(entityData.name);
            }
            if (entityData.transform != null)
            {
                TransformData.TransformDataToTransfrom(entityData.transform, transform);
                transform.hasChanged = false;
            }
        }

        protected virtual string GetDefaultName()
        {
            return "New Entity";
        }

        public virtual void StartDefaultState()
        {
        }

        public virtual void ResetEntityState()
        {

        }

        public virtual bool IsClickable()
        {
            return false;
        }

        public virtual bool IsViewable()
        {
            return true;
        }

        protected void SetId(string id)
        {
            this.id = id;
        }

        public void SetEntityName(string entityName)
        {
            if (!string.IsNullOrEmpty(entityName))
            {
                this.entityName = entityName;
            }
        }

        public string GetId()
        {
            return id;
        }

        public string GetEntityName()
        {
            return entityName;
        }

        protected virtual void OnDestroy()
        {
            GlobalStates.OnIsPlayModeChange -= action;
            OnEntityDestroy?.Invoke(this);
        }
    }
}

