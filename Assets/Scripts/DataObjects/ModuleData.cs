using System;
using System.Collections.Generic;

namespace EAR
{
    [Serializable]
    public class ModuleDataResponse
    {
        public ModuleDataResponseData data;
    }

    [Serializable]
    public class ModuleDataResponseData
    {
        public List<SectionData> sections;
    }

    [Serializable]
    public class SectionData
    {
        public int id;
        public string name;
        public int courseId;
        public int parentSectionId;
        public List<ModuleData> modules = new List<ModuleData>();

        [NonSerialized]
        public Action sectionClickEvent;
        [NonSerialized]
        public List<SectionData> childrenSection = new List<SectionData>();
    }

    [Serializable]
    public class ModuleData
    {
        public int id;
        public string name;
        public string moduleType;
        public ARModuleData arModule;
        public DateTime createdAt;

        [NonSerialized]
        public Action<int> moduleClickEvent;
    }

    [Serializable]
    public class ARModuleData
    {
        public int id;
    }
}

