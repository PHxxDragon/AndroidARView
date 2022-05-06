using EAR.Container;

namespace EAR.Entity.EntityAction
{

    public class StopVideoAction : ButtonAction
    {
        public StopVideoAction(ButtonActionData buttonActionData) : base(buttonActionData)
        {
        }

        public override void ExecuteAction()
        {
            VideoEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId()) as VideoEntity;
            if (entity)
            {
                entity.StopVideo();
            }
        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.StopVideo;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }

}
