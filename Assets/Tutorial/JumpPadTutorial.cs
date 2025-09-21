using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadTutorial : MonoBehaviour
{
    public float jumpForce = 20f;
    Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerTutorialScript playerController = other.GetComponent<PlayerTutorialScript>();
            if (playerController != null) {
                playerController.verticalVelocity = 0f;
                animator.SetTrigger("jumped");
                playerController.verticalVelocity += jumpForce;
            }
        }
    }
}
