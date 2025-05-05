using System.Collections;
using UnityEngine;

public class Scene3Controller : MonoBehaviour
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

    [Header("Animations")]
    [SerializeField] private Animator doorAnimator;

    private string firstButtonClicked = null;
    private bool isIntroFinished = false;

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

        buttonA.onClickAction.AddListener((id) => OnButtonClicked("ButtonALeft"));
        buttonB.onClickAction.AddListener((id) => OnButtonClicked("ButtonBRight"));

    }

    private void HandleDialogueComplete()
    {
        if (!isIntroFinished)
        {
            isIntroFinished = true;
            EnableButtons();
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

        Debug.Log($"[Scene3Controller] Button clicked ID: {buttonID}");
        Debug.Log($"[Scene3Controller] Sender GameObject: {UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject?.name}");

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
            dialogueManager.DisplayNarration(buttonWrongEntry);
        }
        // SECOND CHOICE = CORRECT
        else
        {
            StartCoroutine(PlaySuccessAndLoad());
        }
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool("isOpen", true);
    }

    private IEnumerator PlaySuccessAndLoad()
    {
        dialogueManager.DisplayNarration(buttonRightEntry);
        OpenDoor();

        // Wait until dialogue finishes before loading next room
        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;
        yield return new WaitUntil(() => dialogueFinished);

        yield return new WaitForSeconds(2.0f);
        gameManager.LoadNextRoom();
    }
}