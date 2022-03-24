using UnityEngine;

namespace EAR.View 
{
    public abstract class ViewInterface: MonoBehaviour
    {
        public abstract void Refresh(object args = null);
    }
}
