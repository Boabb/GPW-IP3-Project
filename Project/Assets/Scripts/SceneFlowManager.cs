using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
        "During the German occupation of Hungary in 1944, Budapest's Jewish community faced unprecedented persecution under the alliance between Nazi Germany and the Arrow Cross Party. Before the occupation, Hungarian Jews had been subject to increasing antisemitic laws, but their situation drastically worsened after March 19, when German forces took control. Jewish families were forced into marked “starred houses” and subjected to strict curfews, food shortages, and constant fear of deportation. Despite these hardships, Budapest remained a centre of Jewish resilience, with individuals and organizations—such as diplomats like Raoul Wallenberg—working to provide refuge and forged documents to save lives in the final months of the war.",
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
}
