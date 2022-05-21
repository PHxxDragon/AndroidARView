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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                screenNavigator.GoBack();
            }
        }
    }
}

