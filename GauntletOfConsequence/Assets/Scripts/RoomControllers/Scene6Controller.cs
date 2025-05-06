using System.Collections;
using UnityEngine;

public class Scene6Controller : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameManager gameManager;

    [Header("Chests")]
    [SerializeField] private GameObject chestA;
    [SerializeField] private GameObject chestAEffect;
    [SerializeField] private GameObject chestB;
    [SerializeField] private GameObject chestBEffect;

    [Header("Interactables")]
    [SerializeField] private InteractableObject pizza;
    [SerializeField] private InteractableObject taco;

    [Header("Arrows")]
    [SerializeField] private GameObject directionArrowA;
    [SerializeField] private GameObject directionArrowB;

    [Header("Narration")]
    [SerializeField] private NarrationEntry foodRevealLine;
    [SerializeField] private NarrationEntry finalLine;

    private void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }
        dialogueManager.OnDialogueComplete += HandleDialogueComplete;

        chestA.SetActive(true);
        chestB.SetActive(true);
        pizza.DisableInteraction();
        taco.DisableInteraction();
        directionArrowA.SetActive(false);
        directionArrowB.SetActive(false);
    }

    private void HandleDialogueComplete()
    {
        dialogueManager.OnDialogueComplete -= HandleDialogueComplete;
        RevealFood();
    }

    public void RevealFood()
    {
        StartCoroutine(RevealFoodAfterDialogue());
    }

    public void OnFoodClicked()
    {
        StartCoroutine(PlayFinalAndFinish());
    }

    private IEnumerator RevealFoodAfterDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        // same for second narration
        if (chestAEffect != null)
        {
            chestAEffect.SetActive(true);
            chestAEffect.GetComponent<ParticleSystem>()?.Play();
            chestA.SetActive(false);
        }

        if (chestBEffect != null)
        {
            chestBEffect.SetActive(true);
            chestBEffect.GetComponent<ParticleSystem>()?.Play();
            chestB.SetActive(false);
        }

        //play food reveal narration
        bool dialogueFinished = false;
        System.Action markFinished = () => dialogueFinished = true;
        dialogueManager.OnDialogueComplete += markFinished;
        dialogueManager.DisplayNarration(foodRevealLine);
        yield return new WaitUntil(() => dialogueFinished);
        dialogueManager.OnDialogueComplete -= markFinished;

        //show food + arrows
        pizza.gameObject.SetActive(true);
        pizza.EnableInteraction();
        taco.gameObject.SetActive(true);
        taco.EnableInteraction();
        directionArrowA.SetActive(true);
        directionArrowB.SetActive(true);
    }

    private IEnumerator PlayFinalAndFinish()
    {
        dialogueManager.DisplayNarration(finalLine);

        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;
        yield return new WaitUntil(() => dialogueFinished);
        yield return new WaitForSeconds(2f);
        gameManager.LoadNextRoom();
    }
}
