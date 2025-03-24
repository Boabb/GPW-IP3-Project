using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ContextScreen : MonoBehaviour
{
    public TextMeshProUGUI contextText;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Get the context message and display it
        SceneFlowManager.Instance.StartDisplayingText(contextText);

        // Start fade-in
        StartCoroutine(FadeCanvasGroup(1, fadeDuration));
    }

    public void ProceedToNextScene()
    {
        // Start fade-out and load next scene
        StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        yield return StartCoroutine(FadeCanvasGroup(0, fadeDuration));

        // Retrieve the next level index stored in PlayerPrefs
        int nextLevelIndex = PlayerPrefs.GetInt("NextLevel", 1);

        // Load the next level
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
