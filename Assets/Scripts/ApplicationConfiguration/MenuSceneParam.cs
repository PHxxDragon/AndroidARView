using EAR.View;
using System;

namespace EAR.SceneChange
{
    public class MenuSceneParam
    {
        public static event Action OnLogOut;

        public static int courseId;
        public static int modelId;
        public static string courseName;
        public static int sectionId;

        public static int modelPage;
        public static ModelListView.ModelType modelType;
        public static string modelKeyword;

        public static int coursePage;
        public static CourseListView.CourseType courseType;
        public static string courseKeyword;

        static MenuSceneParam() {
            ResetAll();
        }

        public static void LogOut()
        {
            OnLogOut?.Invoke();
        }

        public static void ResetAll()
        {
            ResetModelList();
            ResetCourseList();
            ResetId();
        }

        public static void ResetCourseList()
        {
            coursePage = 1;
            courseKeyword = "";
            courseType = CourseListView.CourseType.All;
        }

        public static void ResetModelList()
        {
            modelPage = 1;
            modelType = ModelListView.ModelType.All;
            modelKeyword = "";
        }

        public static void ResetId()
        {
            modelId = -1;
            courseId = -1;
            courseName = "";
            sectionId = -1;
        }
    }
}

