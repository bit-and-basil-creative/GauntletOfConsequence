using System.Collections;
using UnityEngine;

public class Scene3Controller : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InteractableObject buttonA;
    [SerializeField] private InteractableObject buttonB;
    [SerializeField] private GameObject directionArrowA;
    [SerializeField] private GameObject directionArrowB;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NarrationEntry buttonWrongEntry;
    [SerializeField] private NarrationEntry buttonRightEntry;
    [SerializeField] private Animator doorAnimator;

    private string firstButtonClicked = null;

    private void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }
        dialogueManager.OnDialogueComplete += HandleDialogueComplete;

        buttonA.DisableInteraction();
        buttonB.DisableInteraction();
        directionArrowA.SetActive(false);
        directionArrowB.SetActive(false);
    }

    private void HandleDialogueComplete()
    {
        EnableButtons();
    }

    public void EnableButtons()
    {
        buttonA.EnableInteraction();
        buttonB.EnableInteraction();
        directionArrowA.SetActive(true);
        directionArrowB.SetActive(true);
    }

    public void OnButtonClicked(string buttonID)
    {
        Debug.Log($"[Scene3Controller] Button clicked ID: {buttonID}");

        if (buttonID == "ButtonALeft")
        {
            buttonA.DisableInteraction();
            directionArrowA.SetActive(false);
            Debug.Log("[Scene3Controller] Disabled ArrowALeft");
        }
        else if (buttonID == "ButtonBRight")
        {
            buttonB.DisableInteraction();
            directionArrowB.SetActive(false);
            Debug.Log("[Scene3Controller] Disabled ArrowBRight");
        }

        // FIRST CHOICE = WRONG
        if (firstButtonClicked == null)
        {
            firstButtonClicked = buttonID;

            // Play "wrong" narration
            dialogueManager.DisplayNarration(buttonWrongEntry);
        }
        // SECOND CHOICE = CORRECT
        else
        {
            // Play "right" narration, THEN load next room
            StartCoroutine(PlaySuccessAndLoad());
        }
    }

    private IEnumerator PlaySuccessAndLoad()
    {
        dialogueManager.DisplayNarration(buttonRightEntry);
        OpenDoor();

        // Wait until dialogue finishes before loading next room
        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;

        yield return new WaitUntil(() => dialogueFinished);
        yield return new WaitForSeconds(2f); // Optional extra delay
        gameManager.LoadNextRoom();
    }

    private void OpenDoor()
    {
        doorAnimator.SetBool("isOpen", true);
    }

}