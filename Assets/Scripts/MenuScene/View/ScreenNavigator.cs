using UnityEngine;
using System.Collections.Generic;

namespace EAR.View
{
    public class ScreenNavigator : MonoBehaviour
    {
        private List<ViewInterface> mainViewStack = new List<ViewInterface>();
        private ViewInterface currentView;

        public bool CanGoBack()
        {
            return mainViewStack.Count != 0;
        }

        public bool IsLoggedIn()
        {
            return currentView && currentView.GetType() != typeof(LoginView);
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
            mainViewStack.Clear();
        }

        public void PushView(ViewInterface newView)
        {
            //TODO: add animation
            mainViewStack.Add(currentView);
            currentView.gameObject.SetActive(false);
            newView.gameObject.SetActive(true);
            currentView = newView;
        }

        public void GoBack()
        {
            if (mainViewStack.Count > 0)
            {
                currentView.gameObject.SetActive(false);
                currentView = mainViewStack[mainViewStack.Count - 1];
                mainViewStack.RemoveAt(mainViewStack.Count - 1);
                currentView.gameObject.SetActive(true);
            }
        }
    }
}
