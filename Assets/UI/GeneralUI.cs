using UnityEngine;
using TMPro;
public class GeneralUI : MonoBehaviour
{
    public static GeneralUI Instance { get; private set; }
    [SerializeField] TextMeshProUGUI infoText;
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
