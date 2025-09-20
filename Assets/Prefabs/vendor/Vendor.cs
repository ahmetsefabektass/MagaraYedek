using System.Collections;
using UnityEngine;

public class Vendor : Interactable
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Interact(PlayerController player)
    {
        if (CanInteract)
        {
            uiController.gameObject.SetActive(false);
            CanInteract = false;
            player.chargingByVendor = true;
            player.HasInteracted = true;
            StartCoroutine(ChargePlayer(player));
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
    IEnumerator ChargePlayer(PlayerController player)
    {
        player.animator.SetTrigger("charging");
        animator.SetTrigger("charging");

        float clipLength = player.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(clipLength);

        while (player.ChargeValue < 100f)
        {
            player.ChargeValue = Mathf.Clamp(player.ChargeValue + Time.deltaTime * 10, 0, 100);
            yield return null;
        }

        player.animator.SetTrigger("chargeDone");
        animator.SetTrigger("charged");

        clipLength = player.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        yield return new WaitForSeconds(clipLength);

        player.chargingByVendor = false;
        CanInteract = true;
        player.HasInteracted = false;
    }


}
