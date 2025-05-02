using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup menuCanvasGroup;

    [SerializeField] private CanvasGroup logoGroup;
    [SerializeField] private RectTransform logoTransform;

    [SerializeField] private CanvasGroup bannerGroup;
    [SerializeField] private RectTransform bannerTransform;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float popScale = 1.1f;
    [SerializeField] private float popDuration = 0.3f;
    [SerializeField] private float delayBetween = 1.0f;

    [Header("Optional SFX")]
    [SerializeField] private AudioSource popSound;

    private void Start()
    {
        //start invisible and normal size
        logoGroup.alpha = 0;
        bannerGroup.alpha = 0;

        logoTransform.localScale = Vector3.one;
        bannerTransform.localScale = Vector3.one;

        StartCoroutine(PlayIntro());
    }

    public void StartGame()
    {
        StartCoroutine(FadeOutAndLoadScene("Game")); // Use scene name or index
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float time = 0f;
        float startAlpha = menuCanvasGroup.alpha;

        while (time < fadeDuration)
        {
            menuCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        menuCanvasGroup.alpha = 0;
        yield return new WaitForSeconds(0.2f); // slight buffer
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlayIntro()
    {
        //animate logo
        yield return StartCoroutine(FadeAndPop(logoGroup, logoTransform));

        //delay before banner appears
        yield return new WaitForSeconds(delayBetween);

        //animate banner
        yield return StartCoroutine(FadeAndPop(bannerGroup, bannerTransform));
    }

    private IEnumerator FadeAndPop(CanvasGroup group, RectTransform rect)
    {
        float time = 0f;
        Vector3 originalScale = Vector3.one;
        Vector3 enlargedScale = originalScale * popScale;

        //fade in
        while (time < fadeDuration)
        {
            group.alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        group.alpha = 1;

        //pop scale up
        time = 0f;
        if (popSound != null) popSound.Play();

        while (time < popDuration)
        {
            rect.localScale = Vector3.Lerp(originalScale, enlargedScale, time / popDuration);
            time += Time.deltaTime;
            yield return null;
        }

        //pop scale back to original
        time = 0f;
        while (time < popDuration)
        {
            rect.localScale = Vector3.Lerp(enlargedScale, originalScale, time / popDuration);
            time += Time.deltaTime;
            yield return null;
        }

        rect.localScale = originalScale;
    }
}
