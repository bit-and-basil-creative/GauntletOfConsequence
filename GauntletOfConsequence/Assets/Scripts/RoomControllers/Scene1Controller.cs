using UnityEngine;
using System.Collections;

public class Scene1Controller : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InteractableObject door;
    [SerializeField] private GameObject directionArrow;
    [SerializeField] private NarratorController narratorController;

    private void Start()
    {

        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        if (narratorController != null)
        {
            StartCoroutine(PlayNarratorIntro());
        }
        dialogueManager.OnDialogueComplete += HandleIntroEnd;
    }

    private void HandleIntroEnd()
    {
        door.EnableInteraction();
        directionArrow.SetActive(true);
    }

    private IEnumerator PlayNarratorIntro()
    {
        narratorController.ClearIdle();
        narratorController.Reappear();
        yield return new WaitForSeconds(2.0f);
        narratorController.Idle();
    }
}
