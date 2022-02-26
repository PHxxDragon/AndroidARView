using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace EAR.View
{
    public class DownloadedFileScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject viewPrefab;
        [SerializeField]
        private GameObject container;

        public void PopulateData(List<DownloadedFileData> downloadedFileDatas)
        {
            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DownloadedFileData data in downloadedFileDatas)
            {
                DownloadedFileView view = Instantiate(viewPrefab, container.transform).GetComponent<DownloadedFileView>();
                view.PopulateData(data);
            }
        }

        public void GoBack()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

}
