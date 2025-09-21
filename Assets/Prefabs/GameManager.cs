using UnityEngine;

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
        timer = 60f;
    }
    void Update()
    {
        TimeHandler();
    }
    public void AddTimer(float time)
    {
        timer += time;
        if (timer > 120f) timer = 120f;
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
}
