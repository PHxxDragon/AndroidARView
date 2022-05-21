using System;
using System.Collections.Generic;
using UnityEngine;

namespace EAR
{
    [Serializable]
    public class ModelListResponse
    {
        public ModelListResponseData data;
    }

    [SerializeField]
    public class ModelDetailResponse
    {
        public ModelDataObject data;
    }

    [Serializable]
    public class ModelListResponseData
    {
        public List<ModelDataObject> models = new List<ModelDataObject>();
        public int pageCount;
    }
    
    [Serializable]
    public class ModelDataObject
    {
        //Serialize JSON
        public int id;
        public int numOfFav;
        public long size;
        public string name;
        public string description;
        public string url;
        public string extension;
        public bool isZipFile;
        public List<string> images = new List<string>();
        public List<string> categories = new List<string>();
        public List<string> tags = new List<string>();

        //Additional properties
        [NonSerialized]
        public List<Action<Sprite>> coverImages;
        [NonSerialized]
        public Action onClick;
    }
}

