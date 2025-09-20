using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image fillerImage;
    public Image EButtonImage;
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            LookToPlayer(Camera.main.transform);
        }
    }
    public void LookToPlayer(Transform player)
    {
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        transform.LookAt(player.position);
        transform.rotation *= rotation;
    }
}
