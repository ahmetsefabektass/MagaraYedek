using UnityEngine;
public abstract class Interactable : MonoBehaviour
{
    public Transform interactionPoint;
    public UIController uiController;
    public bool CanInteract = true;
    public bool isInRange = false;
    public abstract void EnteredRange();
    public abstract void ExitedRange();
    public abstract void Interact(PlayerController player);
}
