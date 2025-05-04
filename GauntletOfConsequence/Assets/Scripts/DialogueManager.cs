using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //events to notify other scripts when dialogue finishes
    public System.Action OnDialogueComplete; //called at the end of any dialogue
    public System.Action OnIntroDialogueComplete; //called only if they dialogue is marked as an intro section

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI dialogueText; //text box that stores dialogue
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private AudioSource audioSource; //for typing or voice sound while dialogue is showing
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject ellipsis; //player clicks to move to the next dialogue box

    //for internal tracking
    private List<string> currentLines; //all dialogue lines for the current entry
    private int currentLineIndex; //track which line we're currently on
    private NarrationEntry currentEntry; //the current dialogue object

    /// <summary>
    /// DisplayNarration method begins a full narration sequence from a NarrationEntry ScriptableObject
    /// </summary>

    public void DisplayNarration(NarrationEntry entry)
    {
        Debug.Log("[DialogueManager] DisplayNarration called with entry: " + entry.name);

        currentEntry = entry;
        currentLines = entry.narrationLines;
        currentLineIndex = 0;
        dialogueText.text = "";
        DisplayNextLine();
    }

    /// <summary>
    /// DisplaySingleLine method plays a single line from the current NarrationEntry by index reference.
    /// </summary>

    public void DisplaySingleLine(int index)
    {
        StopAllCoroutines();
        StartCoroutine(TypewriterEffect(currentEntry.narrationLines[index]));
    }

    /// <summary>
    /// DisplayNextLine displays the next line in the sequence or ends the dialogue if finished.
    /// </summary>

    private void DisplayNextLine()
    {
        if (currentLineIndex < currentLines.Count)
        {
            StopAllCoroutines();
            StartCoroutine(TypewriterEffect(currentLines[currentLineIndex]));
            currentLineIndex++;
        }
        else
        {
            ellipsis.SetActive(false); //hide ellipsis

            if (currentEntry != null && currentEntry.isIntroSection)
            {
                OnIntroDialogueComplete?.Invoke(); //if this is an intro section, call the OnIntroDialogueComplete event
            }

            OnDialogueComplete?.Invoke(); //if this is the last line, call the OnDialogueComplete event
        }
    }

    //coroutine for typewriter effect
    private IEnumerator TypewriterEffect(string line)
    {
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            if (audioSource != null) audioSource.Play(); //sound effect on each character
            yield return new WaitForSeconds(0.05f);
        }

        //rebuild the layout to resize the background container
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialoguePanel.GetComponent<RectTransform>());

        //show ellipsis if there are more dialogue entries
        if (currentLineIndex < currentLines.Count)
        {
            ellipsis.SetActive(true);
        }
        else
        {
            ellipsis.SetActive(false);
            OnDialogueComplete?.Invoke();
        }

    }

    public void OnClickEllipsis()
    {
        ellipsis.SetActive(false);
        DisplayNextLine();
    }
}
