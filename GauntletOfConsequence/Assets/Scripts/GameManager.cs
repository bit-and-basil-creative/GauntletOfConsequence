using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private GameObject[] scenesArray; //array to store scenes

    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private AudioSource audioSource; //background music
    [SerializeField] private NarrationEntry[] narrationSequence;
    [SerializeField] private int[] narrationIndexPerScene;

    private int sceneIndex; //to track which scene we are on

    void Start()
    {
        sceneIndex = 0; //start in scene 1 (intro room)
        LoadSceneByIndex(sceneIndex);
    }

    //enables the scene based on the current index
    void LoadSceneByIndex(int index)
    {
        Debug.Log("[GameManager] Loading scene index: " + index);

        // Disable all scenes
        for (int i = 0; i < scenesArray.Length; i++)
        {
            scenesArray[i].SetActive(false);
        }

        //enable the current scene
        scenesArray[index].SetActive(true);

        if (index < narrationIndexPerScene.Length)
        {
            int narrationIndex = narrationIndexPerScene[index];
            var narration = narrationSequence[narrationIndex];
            Debug.Log($"[GameManager] Narration for this scene: {narration.name}");
            dialogueManager.DisplayNarration(narration);
        }
        else
        {
            Debug.LogWarning("[GameManager] No narration index defined for this scene!");
        }
    }

    //increments to the load the next scene in sequence
    public void LoadNextRoom()
    {
        sceneIndex++;
        LoadSceneByIndex(sceneIndex);
    }

    public NarrationEntry GetNarrationForScene()
    {
        if (sceneIndex >= 0 && sceneIndex < narrationSequence.Length)
        {
            return narrationSequence[sceneIndex];
        }
        else
        {
            Debug.LogWarning("Scene index out of range when requesting narration.");
            return null;
        }
    }
}