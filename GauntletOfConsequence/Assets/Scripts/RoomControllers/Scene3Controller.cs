using System.Collections;
using UnityEngine;

public class Scene3Controller : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameManager gameManager;

    [Header("Interactables")]
    [SerializeField] private InteractableObject buttonA;
    [SerializeField] private InteractableObject buttonB;
    [SerializeField] private GameObject directionArrowA;
    [SerializeField] private GameObject directionArrowB;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioSource doorOpenSound;

    [Header("Dialogue")]
    [SerializeField] private NarrationEntry buttonWrongEntry;
    [SerializeField] private NarrationEntry buttonRightEntry;

    [Header("Narrator")]
    [SerializeField] private NarratorController controller;
    [SerializeField] private GameObject narrator;

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
        doorOpenSound.Play();
        OpenDoor();

        // Wait until dialogue finishes before loading next room
        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;

        yield return new WaitUntil(() => dialogueFinished);
        controller.Disappear();
        yield return new WaitForSeconds(3f); // Optional extra delay
        gameManager.LoadNextRoom();
    }

    private void OpenDoor()
    {
        doorAnimator.SetBool("isOpen", true);
    }

}