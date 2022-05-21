using System;
using System.Collections.Generic;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class CourseListDataResponse
    {
        public CourseListData data;
    }

    [Serializable]
    public class CourseListData
    {
        public List<CourseData> courses;
        public int pageCount;
    }

    [Serializable]
    public class CourseData
    {
        public int id;
        public string name;
        public string about;
        public string type;
        public string accessibility;
        public DateTime startDate;
        public DateTime endDate;
        public string thumbnail;

        [NonSerialized]
        public Action<int, string> courseClickEvent;
        [NonSerialized]
        public Action<Sprite> coverImage;
    }

}
