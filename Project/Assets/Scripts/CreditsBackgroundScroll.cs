using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsBackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 20f;  // Speed of background movement
    public RectTransform backgroundRect1;  // First RectTransform of the background element (first layout group)
    public RectTransform backgroundRect2;  // Second RectTransform of the background element (second layout group)

    private bool isCreditsFinished = false; // To check if both parts of the background have finished scrolling
    private LevelLoader levelLoader; // Reference to the LevelLoader for scene transition

    void Start()
    {
        // Find the LevelLoader in the scene to trigger the scene change later
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    void Update()
    {
        // Move both parts of the background up by the scroll speed each frame
        backgroundRect1.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        backgroundRect2.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        // Check if both background parts have scrolled off the screen
        if (!isCreditsFinished && (backgroundRect1.anchoredPosition.y >= Screen.height || backgroundRect2.anchoredPosition.y >= Screen.height))
        {
            isCreditsFinished = true;
            EndCredits(); // Trigger the scene change once the scroll is complete
        }
    }

    private void EndCredits()
    {
        // Make sure LevelLoader is assigned before calling the method
        if (levelLoader != null)
        {
            // Trigger the scene change when the credits have finished scrolling
            levelLoader.LoadNextLevel();
        }
        else
        {
            Debug.LogError("LevelLoader not found in scene!");
        }
    }

    public void SkipCredits()
    {
        if(!isCreditsFinished)
        {
            Debug.Log("Credits skipped by used.");
            isCreditsFinished=true;
            EndCredits();
        }
    }
}