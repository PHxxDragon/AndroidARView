using EAR.Editor.Presenter;

namespace EAR.SceneChange
{
    public class SceneChangeParam
    {
        public static ModuleARInformation moduleARInformation = new ModuleARInformation();
        static SceneChangeParam(){
            moduleARInformation.extension = "gltf";
            moduleARInformation.imageUrl = "https://library.vuforia.com/sites/default/files/vuforia-library/articles/solution/Magic%20Leap%20Related%20Content/Astronaut-scaled.jpg";
            moduleARInformation.modelUrl = "http://192.168.1.4:4000/wolf_with_animations.zip";
        }
    }
}

