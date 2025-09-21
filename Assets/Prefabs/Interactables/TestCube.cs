using System.Collections;
using UnityEngine;

public class TestCube : Interactable
{
    float x;
    public override void Interact(PlayerController player)
    {
        if (CanInteract)
        {
            x = 20;
            CanInteract = false;
            StartCoroutine(InteractionCoroutine(player));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            EnteredRange();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            ExitedRange();
        }
    }

    public override void EnteredRange()
    {
        uiController.SetActive(true);
    }

    public override void ExitedRange()
    {
        uiController.SetActive(false);
    }

    private IEnumerator InteractionCoroutine(PlayerController player)
    {
        uiController.EButtonImage.enabled = false;
        uiController.fillerImage.fillAmount = 0;
        GeneralUI.Instance.SetInfoText("Hold \"E\" button ", 5f);
        float timer = 0f;
        while (x < 100 && timer < 10f)
        {
            timer += Time.deltaTime;
            if (player.inputs.Player.Interact.WasPressedThisFrame())
            {
                x += 5;
            }
            x -= Time.deltaTime * 15;
            uiController.fillerImage.fillAmount = x / 100;
            yield return null;
        }
        uiController.EButtonImage.enabled = true;
        uiController.fillerImage.fillAmount = 1;
        uiController.gameObject.SetActive(false);
        player.HasInteracted = false;
        GameManager.Instance.ResetTimer();
    }
}
