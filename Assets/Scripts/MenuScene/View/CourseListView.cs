using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EAR.View
{
    public class CourseListView : ListView<CourseView, CourseData>
    {

        public event Action<int, int> CourseListRefreshEvent;

        public override void Refresh(object args = null)
        {
            CourseListRefreshEvent?.Invoke(pageDropdown.value + 1, LIMIT);
        }
    }
}

