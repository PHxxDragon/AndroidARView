using UnityEngine;
using System;

namespace EAR.View
{
    public class ScreenNavigator : MonoBehaviour
    {
        private ViewInterface currentView;
        public event Action<ViewInterface> OnViewChanged;

        public ViewInterface GetCurrentView()
        {
            return currentView;
        }

        public bool CanOpenSideBar()
        {
            return currentView && (currentView is CourseListView || currentView is ModelListView || currentView is SettingView);
        }

        public void OpenView(ViewInterface newView)
        {
            //TODO: add animation
            if (currentView)
            {
                if (newView == currentView)
                    return;
                currentView.gameObject.SetActive(false);
            }
            newView.gameObject.SetActive(true);
            currentView = newView;
            OnViewChanged?.Invoke(currentView);
        }

        public void GoBack()
        {
            if (currentView)
            {
                currentView.GoBack();
            } else
            {
                Debug.LogError("Cannot go back because current view is null");
            }
        }
    }
}
