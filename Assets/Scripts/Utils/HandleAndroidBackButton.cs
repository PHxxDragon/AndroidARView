using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleAndroidBackButton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
