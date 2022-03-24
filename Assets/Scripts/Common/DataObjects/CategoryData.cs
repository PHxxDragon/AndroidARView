using System.Collections.Generic;
using System;

namespace EAR
{
    [Serializable]
    public class CategoryDataResponse
    {
        public List<CategoryDataObject> data;
    }

    [Serializable]
    public class CategoryDataObject
    {
        public int id;
        public string name;
    }
}

