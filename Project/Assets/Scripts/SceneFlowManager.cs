using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    // List of scene indexes to skip the context scene for
    private HashSet<int> scenesToSkipContext = new HashSet<int> { 0, 1, 5 };

    // Array of context messages (matches level index)
    private string[] contextMessages = new string[]
    {
        "Scene 0, no context scene",
        "Scene 1, no context scene",
        "During the German occupation of Hungary in 1944, Budapest's Jewish community faced " +
        "unprecedented persecution under the alliance between Nazi Germany and the Arrow Cross Party. " +
        "Before the occupation, Hungarian Jews had been subject to increasing antisemitic laws, but their " +
        "situation drastically worsened after March 19, when German forces took control. Jewish families " +
        "were forced into marked “starred houses” and subjected to strict curfews, food shortages, and " +
        "constant fear of deportation. Despite these hardships, Budapest remained a centre of Jewish " +
        "resilience, with individuals and organizations—such as diplomats like Raoul Wallenberg—working " +
        "to provide refuge and forged documents to save lives in the final months of the war.",
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
        "ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit " +
        "in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
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

    public void LoadNextLevel()
    {
        // Get the next level index
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Debug log to check the current level index
        Debug.Log("Current Level Index: " + SceneManager.GetActiveScene().buildIndex);

        // Check if we should skip the context scene
        if (scenesToSkipContext.Contains(nextLevelIndex))
        {
            // Skip the context scene and directly load the next level
            Debug.Log("Skipping context scene. Loading next level directly.");
            LoadScene(nextLevelIndex);
        }
        else
        {
            // If context scene is not skipped, show the context screen first
            ShowContextScreenAndLoadNextLevel(nextLevelIndex);
        }
    }

    private void ShowContextScreenAndLoadNextLevel(int nextLevelIndex)
    {
        // Save the next level index to PlayerPrefs for use in ContextScene
        PlayerPrefs.SetInt("NextLevel", nextLevelIndex);
        PlayerPrefs.Save();

        // Load the context scene before the next level
        SceneManager.LoadScene("ContextScene");
    }

    public void LoadScene(int sceneIndex)
    {
        if (sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Scene index out of range!");
        }
    }

    public string GetContextMessage()
    {
        int levelIndex = PlayerPrefs.GetInt("NextLevel", 1);
        if (levelIndex < contextMessages.Length)
        {
            return contextMessages[levelIndex];
        }
        return "Get ready for the next challenge!";
    }

    private List<string> SplitTextIntoChunks(string text)
    {
        List<string> chunks = new List<string>();

        // Split text while keeping punctuation
        string[] sentences = text.Split(new[] { ".", "!", "?" }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string sentence in sentences)
        {
            string trimmedSentence = sentence.Trim();
            if (!string.IsNullOrEmpty(trimmedSentence))
            {
                chunks.Add(trimmedSentence + "."); // Re-add punctuation
            }
        }

        return chunks;
    }

    private List<string> currentTextChunks;
    private int currentChunkIndex = 0;
    private TextMeshProUGUI currentUIText;

    public void StartDisplayingText(TextMeshProUGUI uiText)
    {
        currentUIText = uiText;
        string message = GetContextMessage();
        currentTextChunks = SplitTextIntoChunks(message);
        currentChunkIndex = 0;

        if (currentTextChunks.Count > 0)
        {
            currentUIText.text = currentTextChunks[currentChunkIndex];

            // Show the appropriate sprite for this sentence
            ContextScreen contextScreen = FindObjectOfType<ContextScreen>(); // Get the ContextScreen instance
            if (contextScreen != null)
            {
                contextScreen.UpdateSpriteForSentence(currentChunkIndex); // Pass the current sentence index
            }
        }
    }

    public void ShowNextSentence()
    {
        if (currentTextChunks == null || currentUIText == null) return;

        StartCoroutine(FadeTextOutAndChange());
    }

    private IEnumerator FadeTextOutAndChange()
    {
        // Fade out
        yield return StartCoroutine(FadeTextAlpha(0f, 0.5f));

        currentChunkIndex++;

        if (currentChunkIndex < currentTextChunks.Count)
        {
            // Change the text
            currentUIText.text = currentTextChunks[currentChunkIndex];

            // Update the sprite based on the sentence index
            ContextScreen contextScreen = FindObjectOfType<ContextScreen>();
            if (contextScreen != null)
            {
                contextScreen.UpdateSpriteForSentence(currentChunkIndex); // Pass the updated sentence index
            }

            // Fade in
            yield return StartCoroutine(FadeTextAlpha(1f, 0.5f));
        }
        else
        {
            // Fade out and load the next scene
            yield return StartCoroutine(FadeTextAlpha(0f, 0.5f));
            int nextLevelIndex = PlayerPrefs.GetInt("NextLevel", 1);
            LoadScene(nextLevelIndex);
        }
    }

    private IEnumerator FadeTextAlpha(float targetAlpha, float duration)
    {
        float startAlpha = currentUIText.color.a;
        float time = 0;
        Color textColor = currentUIText.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            textColor.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            currentUIText.color = textColor;
            yield return null;
        }

        textColor.a = targetAlpha;
        currentUIText.color = textColor;
    }

    // Function to call the coroutine from other scripts
    public void DisplayContextMessage(TextMeshProUGUI uiText)
    {
        string message = GetContextMessage();
        StartDisplayingText(uiText);
    }

}
