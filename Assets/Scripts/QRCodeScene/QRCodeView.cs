using UnityEngine;
using UnityEngine.SceneManagement;

public class QRCodeView : MonoBehaviour
{
    public void BackToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
