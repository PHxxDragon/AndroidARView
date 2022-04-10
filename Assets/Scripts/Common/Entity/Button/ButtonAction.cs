namespace EAR.Entity.EntityAction
{
    public abstract class ButtonAction
    {
        private string targetEntityId;

        public ButtonAction(ButtonActionData buttonActionData)
        {
            targetEntityId = buttonActionData.targetEntityId;
        }

        public abstract ButtonActionData GetButtonActionData();

        public void SetTargetEntityId(string entityId)
        {
            targetEntityId = entityId;
        }

        public string GetTargetEntityId()
        {
            return targetEntityId;
        }

        public abstract void ExecuteAction();
    }

}
