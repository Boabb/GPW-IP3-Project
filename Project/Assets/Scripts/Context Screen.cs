using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ContextScreen : MonoBehaviour
{
    public static ContextScreen Instance;

    public RectTransform textTransform;
    public TextMeshProUGUI contextText;
    public CanvasGroup contextCanvasGroup;
    public float fadeDuration = 1f;
    public float scrollSpeed = 50f;
    public float startY = -600f;
    public float endY = 600f;
    public float displayTime = 5f;

    private bool isShowing = false;

    private Dictionary<int, string> levelContextTexts = new Dictionary<int, string>()
    {
        { 2, "A long time ago, in a galaxy far, far away...\n\nThe adventure begins!" },
        { 3, "After escaping the enemy stronghold...\n\nThe hero finds themselves on a strange planet." },
        { 4, "With new allies, the hero prepares for battle...\n\nThe final showdown is near!" }
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Pause the game immediately at the start
        Time.timeScale = 0;

        contextCanvasGroup.alpha = 0;
        contextCanvasGroup.gameObject.SetActive(false);

        // Show the context screen for the current level
        ShowContextScreenForCurrentLevel();
    }

    public void ShowContextScreenForCurrentLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (levelContextTexts.ContainsKey(currentLevel))
        {
            ShowContextScreen(levelContextTexts[currentLevel]);
        }
        else
        {
            StartLevel(); // If no context text is defined, just start the level immediately (optional).
        }
    }

    public void ShowContextScreen(string message)
    {
        if (isShowing) return;

        isShowing = true;
        contextText.text = message;
        contextCanvasGroup.gameObject.SetActive(true);
        textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, startY);

        StartCoroutine(FadeCanvasGroup(1, fadeDuration, () => StartCoroutine(ScrollText())));
    }

    private IEnumerator ScrollText()
    {
        // Scroll the text using unscaledDeltaTime to bypass the time scale being set to 0
        float elapsedTime = 0;
        while (textTransform.anchoredPosition.y < endY)
        {
            textTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.unscaledDeltaTime); // Use unscaledDeltaTime
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Wait for the specified time, using WaitForSecondsRealtime to avoid being affected by Time.timeScale
        yield return new WaitForSecondsRealtime(displayTime);

        // Fade out the context screen
        StartCoroutine(FadeCanvasGroup(0, fadeDuration, () =>
        {
            contextCanvasGroup.gameObject.SetActive(false);
            isShowing = false;

            // Start the level after context screen finishes
            StartLevel();
        }));
    }

    private IEnumerator FadeCanvasGroup(float targetAlpha, float duration, System.Action onComplete = null)
    {
        float startAlpha = contextCanvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime; // Use unscaledDeltaTime here as well
            contextCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        contextCanvasGroup.alpha = targetAlpha;
        onComplete?.Invoke();
    }

    private void StartLevel()
    {
        // Unpause the game after the context screen
        Time.timeScale = 1;

        // You can also manually trigger gameplay-related actions here, like enabling the player or starting AI behaviors.
        Debug.Log("Game resumed. Level starts now.");
    }
}
