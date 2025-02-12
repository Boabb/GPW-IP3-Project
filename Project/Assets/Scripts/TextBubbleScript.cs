using System.Collections;
using UnityEngine;
using TMPro;

public class TextBubbleScript : MonoBehaviour
{
    public TMP_Text textBubble;
    private Coroutine fadeCoroutine;

    public float waitBeforeFade = 5f; // Time before fading starts
    public float fadeDuration = 1.5f; // Actual fade-out duration

    // Method to set text and start fade-out
    public void SetText(string newText)
    {
        if (textBubble != null)
        {
            textBubble.SetText(newText);
            textBubble.alpha = 1f; // Ensure text is fully visible

            // Restart fade-out timer
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeOutText());
        }
        else
        {
            Debug.LogError("TMP_Text component is not assigned in TextBubbleScript!");
        }
    }

    private IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(waitBeforeFade); // Wait before starting the fade

        float elapsed = 0f;
        Color originalColor = textBubble.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            textBubble.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            Debug.Log("Fading... Elapsed Time: " + elapsed);

            yield return null;
        }

        textBubble.SetText(""); // Clear text after fade-out
    }

}
