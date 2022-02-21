using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleAndroidBackButton : MonoBehaviour
{
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}
