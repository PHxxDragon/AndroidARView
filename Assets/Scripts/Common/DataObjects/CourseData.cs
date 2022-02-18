using System;

namespace EAR
{
    [Serializable]
    public class CourseData
    {
        public int id;
        public string title;
        public float rating;
        public string courseType;
        public string teachers;
        public string imageUrl;

        public Action<int> courseClickEvent;
    }

}
