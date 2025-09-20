using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 20f;
    Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null) {
                animator.SetTrigger("jumped");
                playerController.verticalVelocity += jumpForce;
            }
        }
    }
}
