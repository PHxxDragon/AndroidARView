using UnityEngine;

namespace EAR.SceneChange
{
    public class MenuSceneParam
    {
        public static int courseId = -1;
        public static int modelId = -1;
        public static string courseName = "";



        public static void Reset()
        {
            modelId = -1;
            courseId = -1;
            courseName = "";
        }
    }
}

