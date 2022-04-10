using UnityEngine;

namespace EAR.Entity
{
    public class InvisibleEntity : BaseEntity
    {
        public override void ResetEntityState()
        {
            base.ResetEntityState();
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        public override void StartDefaultState()
        {
            base.StartDefaultState();
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public override bool IsViewable()
        {
            return false;
        }
        public override bool IsClickable()
        {
            return false;
        }
    }
}

