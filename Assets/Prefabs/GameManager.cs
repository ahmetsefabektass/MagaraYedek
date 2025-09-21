using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float timer;
    public float Timer { get { return timer; } }
    private bool timeStopped = false;
    public bool TimeStopped { get { return timeStopped; } set { timeStopped = value; } }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        timer = 120;
    }
    void Update()
    {
        TimeHandler();
    }
    public void ResetTimer()
    {
        timer = 120;
    }
    private void TimeHandler()
    {
        if (timeStopped) return;
        timer -= Time.deltaTime;
        GeneralUI.Instance.SetTimerText(timer);
    }
    public void LoadDeathScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ExplodeCutScene");
    }
    public void LoadFinalScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("FinalScene");
    }
}
