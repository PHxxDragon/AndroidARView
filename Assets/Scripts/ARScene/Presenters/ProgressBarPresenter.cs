using UnityEngine;
using EAR.AR;
using EAR.View;

namespace EAR.Editor.Presenter
{
    public class ProgressBarPresenter : MonoBehaviour
    {
        [SerializeField]
        private ImageTargetCreator imageTargetCreator;
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private ProgressBar progressBar;
        [SerializeField]
        private GameObject loadingPanel;

        void Awake()
        {
            if (modelLoader != null && progressBar != null && imageTargetCreator != null)
            {
                imageTargetCreator.CreateTargetStartEvent += () =>
                {
                    progressBar.EnableProgressBar();
                };
                imageTargetCreator.CreateTargetDoneEvent += () =>
                {
                    progressBar.DisableProgressBar();
                };
                imageTargetCreator.OnProgressChanged += (float progress, string text) =>
                {
                    progressBar.SetProgress(progress, text);
                };
                modelLoader.OnLoadStarted += ModelLoadStart;
                modelLoader.OnLoadEnded += ModelLoadEnd;
                modelLoader.OnLoadProgressChanged += ProgressChanged;
            } else
            {
                Debug.Log("Unassigned references");
            }
        }

        private void ModelLoadStart()
        {
            progressBar.EnableProgressBar();
        }

        private void ModelLoadEnd()
        {
            progressBar.DisableProgressBar();
            loadingPanel.SetActive(false);
        }

        private void ProgressChanged(float percent, string text)
        {
            progressBar.SetProgress(percent, text);
        }
    }
}

