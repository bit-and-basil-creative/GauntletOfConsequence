using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene7Controller : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameManager gameManager;
    private void Start()
    {
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        dialogueManager.OnDialogueComplete += HandleDialogueComplete;
    }

    private void HandleDialogueComplete()
    {
        // When the final line of dialogue is finished, return to main menu
        dialogueManager.OnDialogueComplete -= HandleDialogueComplete;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        if (dialogueManager != null)
            dialogueManager.OnDialogueComplete -= HandleDialogueComplete;
    }
}
