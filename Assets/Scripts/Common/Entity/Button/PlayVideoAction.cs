using EAR.Container;

namespace EAR.Entity.EntityAction
{
    public class PlayVideoAction : ButtonAction
    {
        public PlayVideoAction(ButtonActionData buttonActionData) : base(buttonActionData)
        {
        }

        public override void ExecuteAction()
        {
            VideoEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId()) as VideoEntity;
            if (entity)
            {
                entity.PlayVideo();
            }
        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.PlayVideo;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }
}

