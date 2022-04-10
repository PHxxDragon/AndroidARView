using System.Collections.Generic;
using UnityEngine;
using EAR.Container;


namespace EAR.Entity.EntityAction
{
    public class PlaySoundAction : ButtonAction
    {
        public PlaySoundAction(ButtonActionData buttonActionData) : base(buttonActionData)
        {
        }
        public override void ExecuteAction()
        {
            SoundEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId()) as SoundEntity;
            if (entity)
            {
                entity.PlaySound();
            }
        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.PlaySound;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }
}

