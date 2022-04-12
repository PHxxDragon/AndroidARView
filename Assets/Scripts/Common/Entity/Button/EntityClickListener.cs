using UnityEngine;

namespace EAR.Entity.EntityAction
{
    public class EntityClickListener : MonoBehaviour
    {
        private static EntityClickListener instance;
        void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("Two listener instance");
            }
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!GlobalStates.IsMouseRaycastBlocked())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray);
                    float minDistance = float.MaxValue;
                    EntityClickTarget minTarget = null;
                    foreach (RaycastHit hit in hits)
                    {
                        EntityClickTarget target = hit.transform.GetComponentInParent<EntityClickTarget>();
                        if (target && minDistance > hit.distance)
                        {
                            minDistance = hit.distance;
                            minTarget = target;
                        }
                    }
                    if (minTarget)
                    {
                        minTarget.Click();
                    }
                }
            }
        }
    }
}

