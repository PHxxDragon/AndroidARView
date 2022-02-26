using System;

namespace EAR
{
    [Serializable]
    public class DownloadedFileData
    {
        public string title;
        public long size;
        public Action deleteAction;
    }
}
