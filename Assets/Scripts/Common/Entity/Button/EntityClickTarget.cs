using UnityEngine;
using System;

namespace EAR.Entity.EntityAction
{
    public class EntityClickTarget : MonoBehaviour
    {
        public event Action OnEntityClicked;

        public void Click()
        {
            OnEntityClicked?.Invoke();
        }
    }
}

