using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleAndroidBackButton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Go back to menu scene");
            SceneManager.LoadScene("MenuScene");
        }
    }
}
