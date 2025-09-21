using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoosterTutor : MonoBehaviour
{
    bool boosted = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerTutorialScript playerController = other.GetComponent<PlayerTutorialScript>();
            if (playerController != null) {
                Debug.Log("Speed Boost Activated");
                playerController.ActivateSpeedBoost();
                boosted = true;
                StartCoroutine(DeactivateAfterDelay());
            }
        }
    }

    private IEnumerator DeactivateAfterDelay() {
        yield return new WaitForSeconds(1f);
        boosted = false;
    }
}
