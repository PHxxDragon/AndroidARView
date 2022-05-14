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
        public float rating;
        public string courseType;
        public string teachers;
        public string thumbnail;

        [NonSerialized]
        public Action<int> courseClickEvent;
        [NonSerialized]
        public Action<Sprite> coverImage;
    }

}
