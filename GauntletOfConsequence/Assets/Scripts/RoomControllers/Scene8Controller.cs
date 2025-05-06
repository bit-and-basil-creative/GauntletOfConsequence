using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene8Controller : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager.enabled = false;
        dialoguePanel.SetActive(false);
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
