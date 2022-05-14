using UnityEngine;
using EAR.View;

namespace EAR.Presenter
{
    public class HandleBackButtonMenuScreen : MonoBehaviour
    {
        [SerializeField]
        private ScreenNavigator screenNavigator;

        void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (screenNavigator.CanGoBack())
                        screenNavigator.GoBack();
                }
            }
        }
    }
}

