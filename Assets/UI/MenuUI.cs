using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
    public void StartButton()
    {
        if (SceneManager.GetActiveScene().name == "Clock")
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
