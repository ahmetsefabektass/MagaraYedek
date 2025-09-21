using System;
using UnityEngine;
public abstract class Interactable : MonoBehaviour
{
    public Action interacted;
    public Action InteractionFailed;
    public Transform interactionPoint;
    public UIController uiController;
    public bool CanInteract = false;
    public bool isInRange = false;
    public abstract void EnteredRange();
    public abstract void ExitedRange();
    public abstract void Interact(PlayerController player);
}
