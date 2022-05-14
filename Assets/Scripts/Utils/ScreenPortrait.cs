using UnityEngine;

public class ScreenPortrait : MonoBehaviour
{
    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}
