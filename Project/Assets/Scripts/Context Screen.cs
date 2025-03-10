using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ContextScreen : MonoBehaviour
{
    public TextMeshProUGUI contextText;
    public CanvasGroup canvasGroup;
    public float displayTime = 5f;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Get the context message and display it
        contextText.text = SceneFlowManager.Instance.GetContextMessage();

        // Start the process of displaying and loading the next level
        StartCoroutine(DisplayAndLoadNextLevel());
    }

    private IEnumerator DisplayAndLoadNextLevel()
    {
        // Fade in the canvas group
        yield return StartCoroutine(FadeCanvasGroup(1, fadeDuration));

        // Wait for the display time
        yield return new WaitForSeconds(displayTime);

        // Fade out the canvas group
        yield return StartCoroutine(FadeCanvasGroup(0, fadeDuration));

        // Retrieve the next level index stored in PlayerPrefs
        int nextLevelIndex = PlayerPrefs.GetInt("NextLevel", 1);

        // Load the next level (this can be the actual gameplay scene)
        SceneFlowManager.Instance.LoadScene(nextLevelIndex);
    }

    private IEnumerator FadeCanvasGroup(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
