using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2Controller : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InteractableObject buttonA;
    [SerializeField] private InteractableObject buttonB;
    [SerializeField] private GameObject directionArrowA;
    [SerializeField] private GameObject directionArrowB;
    //[SerializeField] private Animator buttonAnimatorA;
    //[SerializeField] private Animator buttonAnimatorB;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NarrationEntry buttonWrongEntry;
    [SerializeField] private NarrationEntry buttonRightEntry;

    private string firstButtonClicked = null;
    private bool isIntroFinished = false;

    private void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        dialogueManager.OnDialogueComplete += HandleDialogueComplete;
    }

    private void HandleDialogueComplete()
    {
        if (!isIntroFinished)
        {
            isIntroFinished = true;
            EnableButtons(); // enable door interaction once intro narration ends
        }
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
        if (buttonID == "ButtonA")
        {
            buttonA.DisableInteraction();
            directionArrowA.SetActive(false);
            //doorAnimatorA.SetBool("isClicked", true);
        }
        else if (buttonID == "ButtonB")
        {
            buttonB.DisableInteraction();
            directionArrowB.SetActive(false);
            //doorAnimatorB.SetBool("isClicked", true);
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

        // Wait until dialogue finishes before loading next room
        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;

        yield return new WaitUntil(() => dialogueFinished);
        yield return new WaitForSeconds(1f); // Optional extra delay
        gameManager.LoadNextRoom();
    }
}

