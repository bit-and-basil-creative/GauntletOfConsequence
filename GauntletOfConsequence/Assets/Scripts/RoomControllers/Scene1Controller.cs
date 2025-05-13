using UnityEngine;
using System.Collections;

public class Scene1Controller : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InteractableObject door;
    [SerializeField] private GameObject directionArrow;
    [SerializeField] private NarratorController controller;
    [SerializeField] private GameObject narrator;

    private void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        dialogueManager.OnDialogueComplete += HandleIntroEnd;
    }

    private void HandleIntroEnd()
    {
        door.EnableInteraction();
        directionArrow.SetActive(true);
        controller.Disappear();
    }

    private void OnDisable()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueComplete -= HandleIntroEnd;
        }
    }
}
