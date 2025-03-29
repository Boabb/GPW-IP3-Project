using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContextScreen : MonoBehaviour
{
    public TextMeshProUGUI contextText;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    // Array of sprites to display
    public Sprite[] contextSprites;

    // Array of durations to control how many sentences each sprite is shown
    public int[] spriteDurations;

    // Reference to the Image component to change its sprite
    public Image contextImage;

    private int currentSentenceIndex = 0; // Track the current sentence index

    private bool isTransitioning = false;

    private void Start()
    {
        // Get the context message and display it
        SceneFlowManager.Instance.StartDisplayingText(contextText);

        // Start fade-in
        StartCoroutine(FadeCanvasGroup(1, fadeDuration));
    }

    public void ProceedToNextScene()
    {
        // Start fade-out and load the next scene
        if (!isTransitioning)
        StartCoroutine(FadeOutAndLoadScene());
        else
        {
            print("Flipping pages");
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeCanvasGroup(0, fadeDuration));

        // Retrieve the next level index stored in PlayerPrefs
        int nextLevelIndex = PlayerPrefs.GetInt("NextLevel", 1);

        // Load the next level
        SceneFlowManager.Instance.LoadScene(nextLevelIndex);

        isTransitioning = false;
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

    // Method to update the sprite based on the current sentence index
    public void UpdateSpriteForSentence(int sentenceIndex)
    {
        // Find the correct sprite index based on sentence count
        int spriteIndex = GetSpriteIndexForSentence(sentenceIndex);

        // Update the image with the selected sprite
        if (spriteIndex >= 0 && spriteIndex < contextSprites.Length)
        {
            contextImage.sprite = contextSprites[spriteIndex];
        }
    }

    // Method to calculate which sprite to display based on the current sentence index
    private int GetSpriteIndexForSentence(int sentenceIndex)
    {
        int totalDuration = 0;

        // Iterate through the durations and sum them up to determine which sprite to use
        for (int i = 0; i < spriteDurations.Length; i++)
        {
            totalDuration += spriteDurations[i];

            // If the sentence index is within the duration range for this sprite, return the sprite index
            if (sentenceIndex < totalDuration)
            {
                return i;
            }
        }

        // If we don't find a matching sprite (e.g., more sentences than we have durations), return the last sprite
        return contextSprites.Length - 1;
    }
}
