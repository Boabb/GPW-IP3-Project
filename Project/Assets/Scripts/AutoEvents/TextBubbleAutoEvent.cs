using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubbleAutoEvent : AutoEvent
{
    public CanvasGroup textBubble; // Assign the CanvasGroup in the Inspector
    public float fadeDuration = 0.5f; // Adjust fade speed

    private void Start()
    {
        if (textBubble != null)
        {
            textBubble.alpha = 0; // Ensure it's hidden at the start
            textBubble.gameObject.SetActive(false);
        }
    }

    public override void EventEnter(GameObject playerGO)
    {
        if (playerGO.CompareTag("Player") && textBubble != null)
        {
            textBubble.gameObject.SetActive(true); // Make sure it's active
            StopAllCoroutines(); // Stop any existing fade-out
            StartCoroutine(FadeCanvasGroup(textBubble, 1, fadeDuration)); // Fade in
        }
    }

    public override void EventExit(GameObject playerGO)
    {
        if (playerGO.CompareTag("Player") && textBubble != null)
        {
            StopAllCoroutines(); // Stop any existing fade-in
            StartCoroutine(FadeCanvasGroup(textBubble, 0, fadeDuration, disableAfterFade: true)); // Fade out
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration, bool disableAfterFade = false)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Ensure it fully reaches target value

        if (disableAfterFade && targetAlpha == 0)
        {
            canvasGroup.gameObject.SetActive(false); // Disable after fading out
        }
    }
}
