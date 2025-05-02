using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private GameObject[] scenesArray; //array to store scenes

    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioSource audioSource; //background music
    [SerializeField] private NarrationEntry[] narrationSequence;

    private int sceneIndex; //to track which scene we are on

    void Start()
    {
        sceneIndex = 0; //start in scene 1 (intro room)
        LoadSceneByIndex(sceneIndex);
    }

    //enables the scene based on the current index
    void LoadSceneByIndex(int index)
    {
        // Disable all scenes
        for (int i = 0; i < scenesArray.Length; i++)
        {
            scenesArray[i].SetActive(false);
        }

        //enable the current scene
        scenesArray[index].SetActive(true);

        //trigger narration for this scene
        dialogueManager.DisplayNarration(narrationSequence[index]);
    }

    //increments to the load the next scene in sequence
    public void LoadNextRoom()
    {
        sceneIndex++;
        LoadSceneByIndex(sceneIndex);
    }
}
