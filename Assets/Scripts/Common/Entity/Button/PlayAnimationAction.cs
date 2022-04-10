using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAR.Container;

namespace EAR.Entity.EntityAction
{
    public class PlayAnimationAction : ButtonAction
    {
        private int animationIndex;

        public PlayAnimationAction(ButtonActionData data) : base(data)
        {
            animationIndex = data.animationIndex;
        }
        public override void ExecuteAction()
        {
            ModelEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId()) as ModelEntity;
            if (entity)
            {
                entity.PlayAnimation(animationIndex);
            }
        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.PlayAnimation;
            buttonActionData.targetEntityId = GetTargetEntityId();
            buttonActionData.animationIndex = animationIndex;
            return buttonActionData;
        }
    }
}

