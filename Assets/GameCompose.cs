using System;
using System.Collections.Generic;
using UnityEngine;

public class GameCompose : MonoBehaviour
{
    [SerializeField] List<Interactable> interactables;
    float timer = 0f;
    Action gameOver;
    int randomIndex;
    bool endlessMode = false;
    void Start()
    {
        foreach (var item in interactables)
        {
            item.interacted += ChangeInteractable;
            item.InteractionFailed += ChangeInteractableFailed;
        }
        randomIndex = UnityEngine.Random.Range(0, interactables.Count);
        SelectInteractable(randomIndex);
    }
    void Update()
    {
        if (!endlessMode) return;
        timer += Time.deltaTime;
        if (timer >= 600)
        {
            gameOver?.Invoke();
        }
    }

    void ChangeInteractable()
    {
        int[] previousIndices = new int[interactables.Count];
        for (int i = 0; i < interactables.Count; i++)
        {
            if (interactables[i].CanInteract == true) continue;
            previousIndices[i] = i;
        }
        randomIndex = UnityEngine.Random.Range(0, previousIndices.Length);
        GameManager.Instance.ResetTimer();
        StartCoroutine(SelectInteractableAfterDelay(randomIndex, 10f));
    }
    System.Collections.IEnumerator SelectInteractableAfterDelay(int i, float delay)
    {
        yield return new WaitForSeconds(delay);
        SelectInteractable(i);
    }
    void ChangeInteractableFailed()
    {
        interactables[randomIndex].CanInteract = true;
    }

    void SelectInteractable(int i)
    {
        interactables[i].CanInteract = true;
        GeneralUI.Instance.SetInfoText("Go and fix: " + interactables[i].name);
    }
    public void SetEndlessMode()
    {
        endlessMode = true;
    }
}
