using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private GameObject[] scenesArray; //array to store scenes

    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private NarrationEntry[] narrationSequence;
    [SerializeField] private int[] narrationIndexPerScene;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sceneMusicClips;

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

        //play scene-specific music
        if (index < sceneMusicClips.Length && sceneMusicClips[index] != null)
        {
            StartCoroutine(SwapMusicWithFade(sceneMusicClips[index], 0.5f)); // 1s fade duration
        }

        if (index < narrationIndexPerScene.Length)
        {
            int narrationIndex = narrationIndexPerScene[index];
            var narration = narrationSequence[narrationIndex];
            Debug.Log($"[GameManager] Narration for this scene: {narration.name}");

            if (index == 0)
            {
                StartCoroutine(DelayedNarration(narration, 1.5f)); //pause
            }
            else
            {
                dialogueManager.DisplayNarration(narration);
            }
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


    //-----------------HELPER METHODS-------------------//

    private IEnumerator DelayedNarration(NarrationEntry entry, float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogueManager.DisplayNarration(entry);
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    private IEnumerator FadeInMusic(AudioClip newClip, float duration)
    {
        audioSource.clip = newClip;
        audioSource.volume = 0f;
        audioSource.Play();

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        audioSource.volume = 0.75f;
    }

    private IEnumerator SwapMusicWithFade(AudioClip newClip, float fadeDuration)
    {
        yield return StartCoroutine(FadeOutMusic(fadeDuration));
        yield return StartCoroutine(FadeInMusic(newClip, fadeDuration));
    }

}