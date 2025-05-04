using System.Collections;
using UnityEngine;

public class Scene5Controller : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private InteractableObject pillarA;
    [SerializeField] private InteractableObject pillarB;
    [SerializeField] private GameObject directionArrowA;
    [SerializeField] private GameObject directionArrowB;
    [SerializeField] private GameObject trapDoor;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NarrationEntry pillarWrongEntry;
    [SerializeField] private NarrationEntry pillarRightEntry;

    private string firstPillarClicked = null;

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
        EnablePillars();
    }

    public void EnablePillars()
    {
        pillarA.EnableInteraction();
        pillarB.EnableInteraction();
        directionArrowA.SetActive(true);
        directionArrowB.SetActive(true);
    }

    public void OnPillarClicked(string pillarID)
    {
        if (pillarID == "PillarA")
        {
            pillarA.DisableInteraction();
            directionArrowA.SetActive(false);
        }
        else if (pillarID == "PillarB")
        {
            pillarB.DisableInteraction();
            directionArrowB.SetActive(false);
        }

        // FIRST CHOICE = WRONG
        if (firstPillarClicked == null)
        {
            firstPillarClicked = pillarID;

            // Play "wrong" narration
            dialogueManager.DisplayNarration(pillarWrongEntry);
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
        trapDoor.SetActive(true); //show the trap door

        dialogueManager.DisplayNarration(pillarRightEntry);
        
        // Wait until dialogue finishes before loading next room
        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;

        yield return new WaitUntil(() => dialogueFinished);
        yield return new WaitForSeconds(2f); // Optional extra delay
        gameManager.LoadNextRoom();
    }
}

