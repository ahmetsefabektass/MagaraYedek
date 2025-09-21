using UnityEngine;
using TMPro;
public class GeneralUI : MonoBehaviour
{
    public static GeneralUI Instance { get; private set; }
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI timerText;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void SetTimerText(float time)
    {
        if (time <= 0f)
        {
            timerText.text = "00:00";
            return;
        }

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void SetInfoText(string text)
    {
        infoText.text = text;
    }
    public void SetInfoText(string text, float duration)
    {
        infoText.text = text;
        CancelInvoke(nameof(ClearInfoText));
        Invoke(nameof(ClearInfoText), duration);
    }

    private void ClearInfoText()
    {
        infoText.text = string.Empty;
    }
}
