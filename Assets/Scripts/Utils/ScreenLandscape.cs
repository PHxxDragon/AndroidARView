using UnityEngine;

public class ScreenLandscape : MonoBehaviour
{
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
