using EAR.Container;

namespace EAR.Entity.EntityAction
{
    public class ShowAction : ButtonAction
    {
        public ShowAction(ButtonActionData buttonActionData): base(buttonActionData)
        {
        }
        public override void ExecuteAction()
        {
            BaseEntity entity = EntityContainer.Instance.GetEntity(GetTargetEntityId());
            if (entity.IsViewable())
            {
                entity.gameObject.SetActive(true);
            }
        }

        public override ButtonActionData GetButtonActionData()
        {
            ButtonActionData buttonActionData = new ButtonActionData();
            buttonActionData.actionType = ButtonActionData.ActionType.Show;
            buttonActionData.targetEntityId = GetTargetEntityId();
            return buttonActionData;
        }
    }
}

