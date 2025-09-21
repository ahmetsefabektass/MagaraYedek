using System.Collections;
using UnityEngine;

public class VendorTutorialScript : InteractableTutorial
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }
    public override void Interact(PlayerController player)
    {

    }
    public override void InteractTutorial(PlayerTutorialScript player)
    {
        Tutorial.Instance.GetBehaviorTree().SendEvent("chargedByVendor");
        if (CanInteract && !player.chargingByVendor && player.ChargeValue < 100f)
        {
            CanInteract = false;
            player.chargingByVendor = true;
            player.HasInteracted = true;
            StartCoroutine(ChargePlayer(player));
            player.chargedByVendor = true;
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
    IEnumerator ChargePlayer(PlayerTutorialScript player)
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
        Tutorial.Instance.GetBehaviorTree().SendEvent("ChargeCompleted");
    }


}
