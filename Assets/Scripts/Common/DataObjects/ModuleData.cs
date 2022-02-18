using System;

namespace EAR
{
    [Serializable]
    public class ModuleData
    {
        public int id;
        public string name;
        public int courseId;
        public int modelId;
        public string metadata;
        public string image;

        public Action<int> moduleClickEvent;
    }
}

