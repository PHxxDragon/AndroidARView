using UnityEngine;

namespace EAR.View 
{
    public abstract class ViewInterface: MonoBehaviour
    {
        public abstract void OpenView(object args = null);
        public abstract void Refresh(object args = null);
        public abstract void CloseView();
    }
}
