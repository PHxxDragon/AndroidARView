using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EAR
{
    public abstract class ListItemView<T> : MonoBehaviour
    {
        public abstract void PopulateData(T data);
    }
}

