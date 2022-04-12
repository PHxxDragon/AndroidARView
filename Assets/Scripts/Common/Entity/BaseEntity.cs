using UnityEngine;
using System;

namespace EAR.Entity
{
    public class BaseEntity : MonoBehaviour
    {
        public static Action<BaseEntity> OnEntityCreated;
        public static Action<BaseEntity> OnEntityChanged;
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

