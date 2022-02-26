using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace EAR.View
{
    public class DownloadedFileView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text size;
        [SerializeField]
        private Button button;

        private Action deleteAction;

        public void PopulateData(DownloadedFileData data)
        {
            title.text = data.title;
            size.text = GetFileSizeString(data.size);
            deleteAction = data.deleteAction;
            button.onClick.AddListener(() =>
            {
                deleteAction?.Invoke();
            });
        }

        private string GetFileSizeString(long size)
        {
            if (size < 1000)
            {
                return size + "b";
            } else if (size < 1000000)
            {
                return ((float)size / 1000).ToString("#.#") + "kb";
            } else
            {
                return ((float)size / 1000000).ToString("#.#") + "mb";
            }
        }
    }
}

