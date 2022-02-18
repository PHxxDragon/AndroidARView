using UnityEngine;
using EAR.View;
using EAR.WebRequest;
using System.Collections.Generic;
using EAR.Editor.Presenter;
using EAR.SceneChange;
using UnityEngine.SceneManagement;

namespace EAR.MenuScene.Presenter
{
    public class ModuleListPresenter : MonoBehaviour
    {
        [SerializeField]
        private ModuleListView moduleListView;

        [SerializeField]
        private WebRequestHelper webRequest;

        void Start()
        {
            moduleListView.ModuleListRefreshEvent += ModuleListRefreshEventSubscriber;
        }

        private void ModuleListRefreshEventSubscriber(int courseId)
        {
            string token = webRequest.GetAuthorizeToken();
            webRequest.GetModuleList(token, courseId, GetModuleListSuccessCallback, null);
        }

        private void GetModuleListSuccessCallback(List<ModuleData> moduleDatas)
        {
            foreach (ModuleData data in moduleDatas)
            {
                data.moduleClickEvent += ModuleClickEventSubscriber;
            }
            moduleListView.PopulateData(moduleDatas);
        }

        private void ModuleClickEventSubscriber(int moduleId)
        {
/*            Param param = new Param();
            param.moduleId = moduleId;
            param.token = webRequest.GetAuthorizeToken();
            SceneChangeParam.param = param;
            SceneChangeParam.isQr = false;
            SceneManager.LoadScene("ARScene");*/
        }
    }
}

