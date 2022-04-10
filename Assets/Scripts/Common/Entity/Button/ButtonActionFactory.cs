namespace EAR.Entity.EntityAction
{
    public class ButtonActionFactory
    {
        public static ButtonAction CreateButtonAction(ButtonActionData buttonActionData)
        {
            switch(buttonActionData.actionType)
            {
                case ButtonActionData.ActionType.Show:
                    return new ShowAction(buttonActionData);
                case ButtonActionData.ActionType.Hide:
                    return new HideAction(buttonActionData);
                case ButtonActionData.ActionType.PlayAnimation:
                    return new PlayAnimationAction(buttonActionData);
                case ButtonActionData.ActionType.PlaySound:
                    return new PlaySoundAction(buttonActionData);
                case ButtonActionData.ActionType.StopSound:
                    return new StopSoundAction(buttonActionData);
                default:
                    return null;
            }
        }
    }
}
