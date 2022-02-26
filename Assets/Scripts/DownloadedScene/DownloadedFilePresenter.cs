using UnityEngine;
using EAR.View;
using EAR.Cacher;
using System.Collections.Generic;

namespace EAR.Presenter
{
    public class DownloadedFilePresenter : MonoBehaviour
    {
        [SerializeField]
        ModelFileCacher modelFileCacher;
        [SerializeField]
        DownloadedFileScreen downloadedFileScreen;

        void Start()
        {
            Refresh();
        }

        private void Refresh()
        {
            var dict = modelFileCacher.GetFileDictionary();
            List<DownloadedFileData> datas = new List<DownloadedFileData>();
            foreach (var keyValue in dict)
            {
                DownloadedFileData data = new DownloadedFileData();
                data.title = keyValue.Value.Item1;
                data.size = keyValue.Value.Item2.Length;
                data.deleteAction += () =>
                {
                    modelFileCacher.RemoveFile(keyValue.Key);
                    Refresh();
                };
                datas.Add(data);
            }
            downloadedFileScreen.PopulateData(datas);
        }
    }
}
