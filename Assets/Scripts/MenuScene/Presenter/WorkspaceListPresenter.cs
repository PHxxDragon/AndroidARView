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
                int id = data.id;
                Utils.Instance.GetImageAsTexture2D(data.imageUrl, (image) =>
                {
                    Sprite sprite = Utils.Instance.Texture2DToSprite(image);
                    workspaceListView.PopulateData(sprite, id);
                }
                , null, null);
            }
        }

        private void WorkspaceClickEventSubscriber(int id)
        {
            //screenNavigator.OpenView(NavigateCommandEnum.ToCourseList, id);
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

