using EAR.Container;

namespace EAR.Entity.EntityAction
{
    public class StopSoundAction : ButtonAction
    {
        public StopSoundAction(ButtonActionData buttonActionData) : base(buttonActionData)
        {
        }
        public override void ExecuteAction()
        {
            SoundEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId()) as SoundEntity;
            if (entity)
            {
                entity.StopSound();
            }
        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.StopSound;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }
}

