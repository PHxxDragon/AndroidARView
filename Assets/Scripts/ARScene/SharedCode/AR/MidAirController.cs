
using UnityEngine;
using Vuforia;

namespace EAR.AR
{
    public class MidAirController : MonoBehaviour
    {
        [SerializeField]
        private GameObject modelContainer;
        [SerializeField]
        private MidAirPositionerBehaviour midAirPositionerBehaviour;

        public void PerformClick(Vector2 position)
        {
            midAirPositionerBehaviour.ConfirmAnchorPosition(position);
        }

        public void AdjustModelPosition()
        {
            if (modelContainer != null)
            {
                Bounds bounds = Utils.GetModelBounds(modelContainer);
                if (bounds.size == Vector3.zero)
                {
                    return;
                }
                modelContainer.transform.localPosition = new Vector3(0, -bounds.center.y, 0);
            } else
            {
                Debug.Log("Unassigned references");
            }
        }
    }
}

