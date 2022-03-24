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
            size.text = Utils.GetFileSizeString(data.size);
            deleteAction = data.deleteAction;
            button.onClick.AddListener(() =>
            {
                deleteAction?.Invoke();
            });
        }
    }
}

