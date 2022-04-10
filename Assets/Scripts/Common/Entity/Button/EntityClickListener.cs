using UnityEngine;
using System;

namespace EAR.Entity.EntityAction
{
    public class EntityClickListener : MonoBehaviour
    {
        public event Action OnEntityClicked;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!GlobalStates.IsMouseRaycastBlocked())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.transform.GetComponentInParent<EntityClickListener>() == this)
                        {
                            OnEntityClicked?.Invoke();
                        }
                    }
                }
            }
        }
    }
}

