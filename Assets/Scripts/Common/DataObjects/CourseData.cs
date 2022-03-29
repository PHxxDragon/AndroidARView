using System;
using System.Collections.Generic;

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
        public string imageUrl;

        public Action<int> courseClickEvent;
    }

}
