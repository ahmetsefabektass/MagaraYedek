using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuUI : MonoBehaviour
{
    [SerializeField]Image background;
    public void StartButton()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            HideMenu();
            return;
        }
        SceneManager.LoadScene("CutScene");
    }
    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void ShowMenu()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            Color color = background.color;
            color.a = 0f;
            background.color = color;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
    public void HideMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
