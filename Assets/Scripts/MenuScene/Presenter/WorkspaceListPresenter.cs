using UnityEngine;
using EAR.View;
using EAR.WebRequest;
using System.Collections.Generic;

namespace EAR.MenuScene.Presenter
{
    public class WorkspaceListPresenter : MonoBehaviour
    {
        [SerializeField]
        private WorkspaceListView workspaceListView;

        [SerializeField]
        private WebRequestHelper webRequest;

        [SerializeField]
        private ScreenNavigator screenNavigator;

        [SerializeField]
        private CourseListView courseListView;

        void Start()
        {
            workspaceListView.WorkspaceRefreshEvent += WorkspaceRefreshEventSubscriber;
        }

        private void WorkspaceRefreshEventSubscriber()
        {
            string token = webRequest.GetAuthorizeToken();
            webRequest.GetWorkspaceList(token, GetWorkspaceListSuccessCallback, null);
        }

        private void GetWorkspaceListSuccessCallback(List<WorkspaceData> workspaceDatas)
        {
            foreach (WorkspaceData data in workspaceDatas)
            {
                data.workspaceClickEvent = WorkspaceClickEventSubscriber;
            }
            workspaceListView.PopulateData(workspaceDatas);
            foreach (WorkspaceData data in workspaceDatas)
            {
                Utils.Instance.GetImageAsTexture2D(data.imageUrl, RetrieveImageSuccessCallback, null, null, data.id);
            }
        }

        private void RetrieveImageSuccessCallback(Texture2D image, object param)
        {
            Sprite sprite = Utils.Instance.Texture2DToSprite(image);
            workspaceListView.PopulateData(sprite, (int)param);
        }

        private void WorkspaceClickEventSubscriber(int id)
        {
            screenNavigator.OpenView(NavigateCommandEnum.ToCourseList, id);
            if (courseListView != null)
            {
                courseListView.Refresh();
            }
            else
            {
                Debug.LogWarning("Unassigned reference");
            }

        }
    }
}

