using UnityEngine;
using EAR.AR;
using EAR.View;

namespace EAR.Editor.Presenter
{
    public class ModelLoaderToProgressBar : MonoBehaviour
    {
        [SerializeField]
        private ModelLoader modelLoader;
        [SerializeField]
        private ProgressBar progressBar;

        void Awake()
        {
            if (modelLoader != null && progressBar != null)
            {
                modelLoader.OnLoadStarted += ModelLoadStart;
                modelLoader.OnLoadEnded += ModelLoadEnd;
                modelLoader.OnLoadProgressChanged += ProgressChanged;
            }
        }

        private void ModelLoadStart()
        {
            progressBar.EnableProgressBar();
        }

        private void ModelLoadEnd()
        {
            progressBar.DisableProgressBar();
        }

        private void ProgressChanged(float percent, string text)
        {
            progressBar.SetProgress(percent, text);
        }
    }
}

