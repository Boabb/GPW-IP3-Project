using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    public bool isTransitioning = false;

    // Enum to represent the scene names
    public enum SceneIndex
    {
        MainMenu,
        ControlScreen,
        Level1,
        Level2,
        Level3,
        ContextScreen
    }

    // List of scene names to skip the context scene for
    private HashSet<SceneIndex> scenesToSkipContext = new HashSet<SceneIndex>
    {
        SceneIndex.MainMenu,
        SceneIndex.ControlScreen,
        SceneIndex.ContextScreen
    };

    // Array of context messages (using the SceneIndex enum)
    private Dictionary<SceneIndex, string> contextMessages = new Dictionary<SceneIndex, string>
    {
        { SceneIndex.MainMenu, "Scene 0, no context scene" },
        { SceneIndex.ControlScreen, "Scene 1, no context scene" },
        { SceneIndex.Level1, "During the German occupation of Hungary in 1944, Budapest's Jewish community faced unprecedented persecution under the alliance between Nazi Germany and the Arrow Cross Party. Before the occupation, Hungarian Jews had been subject to increasing antisemitic laws, but their situation drastically worsened after March 19, when German forces took control. Jewish families were forced into marked “starred houses” and subjected to strict curfews, food shortages, and constant fear of deportation. Despite these hardships, Budapest remained a centre of Jewish resilience, with individuals and organizations—such as diplomats like Raoul Wallenberg—working to provide refuge and forged documents to save lives in the final months of the war." },
        { SceneIndex.Level2, "When the Nazis and the Arrow Cross Party, a Hungarian ultranationalist party who worked with the Nazis, took over Hungary on the 15th of October 1944, Jews began to be moved from the starred houses into the ghetto or to protected houses. Protected houses were provided by states that were neutral in World War II, like Sweden and the Vatican and served as a place for Jews who had ‘protection papers’ to live. Protection papers were issued by neutral states to prevent deportation of Jewish people to concentration camps." },
        { SceneIndex.Level3, "A Swedish diplomat named Raoul Wallenberg protected many Jews in houses in Budapest by declaring them Swedish territory. Around Christmas 1944, Soviet forces began the Siege of Budapest. Many civilians died during the siege and thousands of Jews were executed by the Arrow Cross Party. The Siege ended in Soviet victory on the 13th of February 1945, and the Nazis were driven out of Budapest." }
    };

    // Updated class to store both sprites and their corresponding durations
    [System.Serializable]
    public class LevelSprites
    {
        public SceneIndex sceneIndex; // Using SceneIndex enum
        public List<Sprite> sprites; // List of sprites for this level
        public List<int> durations; // List of durations for each sprite
    }

    // List of LevelSprites, editable in the Inspector
    public List<LevelSprites> levelSpritesDictionary;

    private List<string> currentTextChunks;
    private int currentChunkIndex = 0;

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
        if (isTransitioning) return;

        // Get the next level index
        SceneIndex nextLevelIndex = (SceneIndex)(SceneManager.GetActiveScene().buildIndex + 1);

        // Check if we should skip the context scene
        if (scenesToSkipContext.Contains(nextLevelIndex))
        {
            LoadScene(nextLevelIndex);
        }
        else
        {
            ShowContextScreenAndLoadNextLevel(nextLevelIndex);
        }
    }

    private void ShowContextScreenAndLoadNextLevel(SceneIndex nextLevelIndex)
    {
        if (isTransitioning) return;

        isTransitioning = true;

        // Save the next level index to PlayerPrefs for use in ContextScene
        PlayerPrefs.SetInt("NextLevel", (int)nextLevelIndex);
        PlayerPrefs.Save();

        // Load the context scene before the next level
        SceneManager.LoadScene("ContextScene");

        StartCoroutine(ResetTransitionFlag());
    }

    private IEnumerator ResetTransitionFlag()
    {
        yield return new WaitForSeconds(1f);
        isTransitioning = false;
    }

    public void LoadScene(SceneIndex sceneIndex)
    {
        if (isTransitioning) return;

        if ((int)sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex.ToString());
        }
        else
        {
            Debug.LogError("Scene index out of range!");
        }
    }

    public string GetContextMessage()
    {
        SceneIndex levelIndex = (SceneIndex)PlayerPrefs.GetInt("NextLevel", 1);

        if (contextMessages.ContainsKey(levelIndex))
        {
            return contextMessages[levelIndex];
        }

        return "Get ready for the next challenge!";
    }

    private LevelSprites GetLevelSprites(SceneIndex sceneIndex)
    {
        // Look for the level in the dictionary and return the sprites and durations
        foreach (var levelSprites in levelSpritesDictionary)
        {
            if (levelSprites.sceneIndex == sceneIndex)
            {
                return levelSprites;
            }
        }
        return null; // Return null if no data is found for this level
    }

    private int GetSpriteForSentence(int sentenceIndex)
    {
        // Get the current scene index
        SceneIndex sceneIndex = (SceneIndex)PlayerPrefs.GetInt("NextLevel", 0);

        // Get the sprites and durations for the current scene
        LevelSprites levelData = GetLevelSprites(sceneIndex);

        if (levelData == null) return -1;

        int totalDuration = 0;

        // Iterate through the durations for the current scene
        for (int i = 0; i < levelData.durations.Count; i++)
        {
            totalDuration += levelData.durations[i];

            // Select the appropriate sprite for this sentence based on the duration
            if (sentenceIndex < totalDuration)
            {
                return i; // Return the sprite index for this scene
            }
        }

        // If no valid sprite is found, return the default sprite
        return Mathf.Min(levelData.sprites.Count - 1, levelData.durations.Count - 1);
    }

    public List<string> SplitTextIntoChunks(string text)
    {
        List<string> chunks = new List<string>();

        // Split the text into sentences using punctuation marks as delimiters
        string[] sentences = text.Split(new[] { '.', '!', '?' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string sentence in sentences)
        {
            string trimmedSentence = sentence.Trim();
            if (!string.IsNullOrEmpty(trimmedSentence))
            {
                // Add the sentence with proper punctuation back to the list
                chunks.Add(trimmedSentence + ".");
            }
        }

        return chunks;
    }

    public void StartDisplayingText(TextMeshProUGUI uiText)
    {
        string message = GetContextMessage();
        currentTextChunks = SplitTextIntoChunks(message); // Use the globally defined currentTextChunks
        currentChunkIndex = 0; // Ensure starting from 0 every time

        // Get sprite data for the scene
        LevelSprites levelData = GetLevelSprites((SceneIndex)PlayerPrefs.GetInt("NextLevel", 0));

        // Pass data to ContextScreen
        ContextScreen contextScreen = FindObjectOfType<ContextScreen>();
        if (contextScreen != null && currentTextChunks.Count > 0)
        {
            contextScreen.InitializeContextScreen(currentTextChunks, levelData.sprites.ToArray(), levelData.durations.ToArray());
        }
    }


    public void ShowNextSentence()
    {
        if (currentTextChunks == null || currentChunkIndex >= currentTextChunks.Count) return;

        currentChunkIndex++;

        if (currentChunkIndex >= currentTextChunks.Count)
        {
            TransitionToNextScene();
        }
        else
        {
            ContextScreen contextScreen = FindObjectOfType<ContextScreen>();
            if (contextScreen != null && currentChunkIndex < currentTextChunks.Count)
            {
                int spriteIndex = GetSpriteForSentence(currentChunkIndex);
                LevelSprites levelData = GetLevelSprites((SceneIndex)PlayerPrefs.GetInt("NextLevel", 0));
                Sprite spriteToDisplay = levelData.sprites[spriteIndex];
                contextScreen.UpdateTextAndSprite(currentTextChunks[currentChunkIndex], spriteToDisplay);
            }
        }

        PlayerPrefs.SetInt("CurrentChunkIndex", currentChunkIndex);
        PlayerPrefs.Save();
    }

    private void TransitionToNextScene()
    {
        ContextScreen contextScreen = FindObjectOfType<ContextScreen>();
        if (contextScreen != null)
        {
            contextScreen.ProceedToNextScene();
        }
    }
}
