using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Transition Settings")]
    public Animator transition; // Animator for the transition animation
    public float transitionTime = 1f; // Duration for the transition effect

    [Header("Loading Options")]
    public bool useSceneFlowManager = true; // Toggle in Inspector to choose method

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the trigger: " + other.gameObject.name); // Debug Log

        // Check if the object entering the collider is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger!"); // Debug Log
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        Debug.Log("Loading next level..."); // Debug Log
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        Debug.Log("Playing transition animation..."); // Debug Log

        // Trigger the transition animation
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }
        else
        {
            Debug.LogWarning("No transition animator assigned!");
        }

        // Wait for the transition animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Decide whether to use SceneFlowManager or load directly
        if (useSceneFlowManager)
        {
            if (SceneFlowManager.Instance != null)
            {
                Debug.Log("Using SceneFlowManager to load the next level...");
                SceneFlowManager.Instance.LoadNextLevel();
            }
            else
            {
                Debug.LogError("SceneFlowManager.Instance is null! Cannot load scene.");
            }
        }
        else
        {
            Debug.Log("Directly loading menu ");
            SceneManager.LoadScene(0);
        }
    }
}
