using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene4Controller : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameManager gameManager;

    [Header("Potions")]
    [SerializeField] private InteractableObject potionA; //blue potion
    [SerializeField] private InteractableObject potionB; //yellow potion
    [SerializeField] private InteractableObject potionC; //purple potion
    [SerializeField] private GameObject potionEffect;
    [SerializeField] private AudioSource potionEffectSource;

    [Header("Arrows")]
    [SerializeField] private GameObject directionArrowA;
    [SerializeField] private GameObject directionArrowB;

    [Header("Narration")]
    [SerializeField] private NarrationEntry potionWrongEntry;
    [SerializeField] private NarrationEntry potionRightEntry;
    [SerializeField] private NarrationEntry potionCEntry;
    [SerializeField] private NarrationEntry potionFinalEntry;

    [Header("Narrator")]
    [SerializeField] private NarratorController controller;
    [SerializeField] private GameObject narrator;

    [Header("Animations")]
    [SerializeField] private Animator doorAnimator;

    private string firstPotionClicked = null;

    private void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        dialogueManager.OnDialogueComplete += HandleDialogueComplete;

        potionA.DisableInteraction();
        potionB.DisableInteraction();
        potionC.DisableInteraction();
        directionArrowA.SetActive(false);
        directionArrowB.SetActive(false);
    }

    private void HandleDialogueComplete()
    {
        EnablePotions();
    }

    public void EnablePotions()
    {
        potionA.EnableInteraction();
        potionB.EnableInteraction();
        directionArrowA.SetActive(true);
        directionArrowB.SetActive(true);
    }

    public void OnPotionClicked(string potionID)
    {
        if (potionID == "PotionA")
        {
            potionA.DisableInteraction();
            potionA.gameObject.SetActive(false);
            directionArrowA.SetActive(false);
        }
        else if (potionID == "PotionB")
        {
            potionB.DisableInteraction();
            potionB.gameObject.SetActive(false);
            directionArrowB.SetActive(false);
        }

        // FIRST CHOICE = WRONG
        if (firstPotionClicked == null)
        {
            firstPotionClicked = potionID;

            // Play "wrong" narration
            dialogueManager.DisplayNarration(potionWrongEntry);
        }
        // SECOND CHOICE = CORRECT
        else
        {
            //show 3rd potion option after dialogue finishes
            StartCoroutine(RevealPotionCAfterDialogue());
        }
    }
    public void OnPotionCClicked () 
    {
        StartCoroutine(PlayFinalLineAndLoad());
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool("isOpen", true);
    }

    private IEnumerator RevealPotionCAfterDialogue() 
    {
        //play the narrator line
        dialogueManager.DisplayNarration(potionRightEntry);

        //wait for that narration to finish
        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;
        yield return new WaitUntil(() => dialogueFinished);

        potionA.DisableInteraction();
        potionA.gameObject.SetActive(false);
        directionArrowA.SetActive(false);
        potionB.DisableInteraction();
        potionB.gameObject.SetActive(false);
        directionArrowB.SetActive(false);

        controller.CastSpell();
        //pause before potion appears
        yield return new WaitForSeconds(1.0f);

        //show the potion appear effect and make Potion C visible + clickable
        if (potionEffect != null) potionEffect.SetActive(true);
        potionEffect.GetComponent<ParticleSystem>()?.Play();
        potionEffectSource.Play();
        potionC.gameObject.SetActive(true);
        potionC.EnableInteraction();

        //start the next bit of narration
        dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;
        dialogueManager.DisplayNarration(potionCEntry);
        yield return new WaitUntil(() => dialogueFinished);
    }
    
    private IEnumerator PlayFinalLineAndLoad()
    {
        dialogueManager.DisplayNarration(potionFinalEntry);
        OpenDoor();

        bool dialogueFinished = false;
        dialogueManager.OnDialogueComplete += () => dialogueFinished = true;
        yield return new WaitUntil(() => dialogueFinished);
        controller.Disappear();
        yield return new WaitForSeconds(3f);
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
