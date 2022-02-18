using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EAR.View
{
    public class ScreenNavigator : MonoBehaviour
    {
        [SerializeField]
        private ViewInterface sidebar;

        [SerializeField]
        private ViewInterface login;

        [SerializeField]
        private ViewInterface workspaceList;
        [SerializeField]
        private ViewInterface courseList;
        [SerializeField]
        private ViewInterface moduleList;

        [SerializeField]
        private ViewInterface profile;
        [SerializeField]
        private ViewInterface setting;


        private List<ViewInterface> mainViewStack = new List<ViewInterface>();
        private ViewInterface currentView;

        public void OpenView(NavigateCommandEnum command, object args = null)
        {
            switch(command)
            {
                case NavigateCommandEnum.ToLogin:
                    if (currentView != login)
                    {
                        mainViewStack.Clear();
                        currentView.CloseView();
                        login.OpenView(args);
                        currentView = login;
                    }
                    break;
                case NavigateCommandEnum.ToSidebar:
                    sidebar.OpenView(args);
                    break;
                case NavigateCommandEnum.ToWorkspaceList:
                    if (currentView != workspaceList)
                    {
                        currentView.CloseView();
                        workspaceList.OpenView(args);
                        mainViewStack.Add(workspaceList);
                        currentView = workspaceList;
                    }
                    break;
                case NavigateCommandEnum.ToCourseList:
                    if (currentView != courseList)
                    {
                        currentView.CloseView();
                        courseList.OpenView(args);
                        mainViewStack.Add(courseList);
                        currentView = courseList;
                    }
                    break;
                case NavigateCommandEnum.ToModuleList:
                    if (currentView != moduleList)
                    {
                        currentView.CloseView();
                        moduleList.OpenView(args);
                        mainViewStack.Add(moduleList);
                        currentView = moduleList;
                    }
                    break;
                case NavigateCommandEnum.ToProfile:
                    if (currentView != profile)
                    {
                        currentView.CloseView();
                        profile.OpenView(args);
                        currentView = profile;
                    }
                    break;
                case NavigateCommandEnum.ToSetting:
                    if (currentView != setting)
                    {
                        currentView.CloseView();
                        setting.OpenView(args);
                        currentView = setting;
                    }
                    break;
                case NavigateCommandEnum.ToMainStack:
                    if (!isInMainStack(currentView) && mainViewStack.Count > 0)
                    {
                        currentView.CloseView();
                        currentView = mainViewStack[mainViewStack.Count - 1];
                        currentView.OpenView(args);
                    }
                    break;
            }
        }

        public void GoBack()
        {
            if (isInMainStack(currentView) && mainViewStack.Count > 1)
            {
                currentView.CloseView();
                mainViewStack.Remove(currentView);
                currentView = mainViewStack[mainViewStack.Count - 1];
                currentView.OpenView();
            }
        }

        void Start()
        {
            currentView = login;
        }

        private bool isInMainStack(ViewInterface viewInterface)
        {
            return viewInterface == workspaceList || viewInterface == courseList || viewInterface == moduleList;
        }
    }
}
