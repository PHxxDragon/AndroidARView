using System;

namespace EAR
{
    [Serializable]
    public class WorkspaceData
    {
        public int id;
        public string name;
        public string role;
        public string imageUrl;
        public string owner;

        public Action<int> workspaceClickEvent;

        public WorkspaceData Copy()
        {
            WorkspaceData obj = new WorkspaceData();
            obj.id = id;
            obj.name = name;
            obj.role = role;
            obj.imageUrl = imageUrl;
            obj.owner = owner;
            obj.workspaceClickEvent = workspaceClickEvent;
            return obj;
        }
    }
}

