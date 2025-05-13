using UnityEngine;
using System.Collections;

public class Scene2Controller : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameManager gameManager;

    [Header("Interactables")]
    [SerializeField] private InteractableObject doorA;
    [SerializeField] private InteractableObject doorB;
    [SerializeField] private GameObject closetA;
    [SerializeField] private GameObject closetB;
    [SerializeField] private GameObject directionArrowA;
    [SerializeField] private GameObject directionArrowB;
    [SerializeField] private Animator doorAnimatorA;
    [SerializeField] private Animator doorAnimatorB;

    [Header("Dialogue")]
    [SerializeField] private NarrationEntry doorWrongEntry;
    [SerializeField] private NarrationEntry doorRightEntry;

    [Header("Narrator")]
    [SerializeField] private NarratorController controller;
    [SerializeField] private GameObject narrator;

    private string firstDoorClicked = null;
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
            EnableDoors(); // enable door interaction once intro narration ends
        }
    }

    public void EnableDoors()
    {
        doorA.EnableInteraction();
        doorB.EnableInteraction();
        directionArrowA.SetActive(true);
        directionArrowB.SetActive(true);
    }

    public void OnDoorClicked(string doorID)
    {
        if (doorID == "DoorA")
        {
            doorA.DisableInteraction();
            directionArrowA.SetActive(false);
            doorAnimatorA.SetBool("isClicked", true);
        }
        else if (doorID == "DoorB")
        {
            doorB.DisableInteraction();
            directionArrowB.SetActive(false);
            doorAnimatorB.SetBool("isClicked", true);
        }

        // FIRST CHOICE = WRONG
        if (firstDoorClicked == null)
        {
            firstDoorClicked = doorID;

            // Show closet behind the clicked door
            if (doorID == "DoorA") closetA.SetActive(true);
            else closetB.SetActive(true);

            // Play "wrong" narration
            dialogueManager.DisplayNarration(doorWrongEntry);
        }
        // SECOND CHOICE = CORRECT
        else
        {
            // Show closet behind first door only
            if (firstDoorClicked == "DoorA") closetB.SetActive(false);
            else closetA.SetActive(false);

            // Play "right" narration, THEN load next room
            StartCoroutine(PlaySuccessAndLoad());
        }
    }

    private IEnumerator PlaySuccessAndLoad()
    {
        dialogueManager.DisplayNarration(doorRightEntry);

        // Wait until dialogue finishes before loading next room
        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;

        yield return new WaitUntil(() => dialogueFinished);
        controller.Disappear();
        yield return new WaitForSeconds(3f); // Optional extra delay

        gameManager.LoadNextRoom();
    }

    private void OnDisable()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueComplete -= HandleDialogueComplete;
        }
    }

}