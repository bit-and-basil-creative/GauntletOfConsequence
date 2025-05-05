using UnityEngine;
using System.Collections;

public class Scene1Controller : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InteractableObject door;
    [SerializeField] private GameObject directionArrow;

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
    }

    private void OnDisable()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueComplete -= HandleIntroEnd;
        }
    }
}
