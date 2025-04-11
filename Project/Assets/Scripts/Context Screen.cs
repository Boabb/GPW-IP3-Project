using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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

    private bool isTransitioning = false;
    private List<string> currentTextChunks; // To hold chunks of text for each sentence
    private int currentChunkIndex = 0;

    private void Start()
    {
        // Initialize context text and sprite with the data from SceneFlowManager
        SceneFlowManager.Instance.StartDisplayingText(contextText);

        // Start fade-in
        StartCoroutine(FadeCanvasGroup(1, fadeDuration));
    }

    public void InitializeContextScreen(List<string> textChunks, Sprite[] sprites, int[] durations)
    {
        currentTextChunks = textChunks;
        contextSprites = sprites;
        spriteDurations = durations;

        // Show the first chunk of text and sprite
        UpdateTextAndSprite(currentTextChunks[currentChunkIndex], contextSprites[GetSpriteIndexForSentence(currentChunkIndex)]);
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

        // Use the SceneFlowManager to load the scene
        SceneFlowManager.Instance.LoadScene((SceneFlowManager.SceneIndex)nextLevelIndex);

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

    public void UpdateTextAndSprite(string newText, Sprite newSprite)
    {
        contextText.text = newText;
        contextImage.sprite = newSprite;
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

        // If we don't find a matching sprite return the last sprite
        return contextSprites.Length - 1;
    }

    public void ShowNextSentence()
    {
        // Check if there are more text chunks to show
        if (currentTextChunks == null || currentChunkIndex >= currentTextChunks.Count)
            return;

        // Increment the chunk index to move to the next sentence
        currentChunkIndex++;

        // If there are more sentences, show the next one
        if (currentChunkIndex < currentTextChunks.Count)
        {
            UpdateTextAndSprite(currentTextChunks[currentChunkIndex], contextSprites[GetSpriteIndexForSentence(currentChunkIndex)]);
        }
        else
        {
            // If we've reached the last sentence, transition to the next scene
            ProceedToNextScene();
        }
    }
}
