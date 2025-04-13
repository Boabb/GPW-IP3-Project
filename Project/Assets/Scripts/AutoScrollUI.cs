using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScrollUI : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollDuration = 10f; // How long it takes to scroll fully
    private float timeElapsed = 0f;
    private bool isScrolling = true;

    [Header("Level Loading Settings")]
    public LevelLoader levelLoader; // Reference to the LevelLoader script

    void Update()
    {
        if (scrollRect != null && isScrolling)
        {
            // Increase timeElapsed based on deltaTime
            timeElapsed += Time.deltaTime;

            // Calculate the normalized position (0 to 1) based on scrollDuration
            float normalizedPosition = Mathf.Clamp01(timeElapsed / scrollDuration);

            // Apply the normalized position to ScrollRect
            scrollRect.horizontalNormalizedPosition = normalizedPosition;

            // If the scrolling has finished (i.e., reached position 1)
            if (normalizedPosition >= 1f)
            {
                isScrolling = false; // Stop scrolling after reaching the end
                LoadNextLevel(); // Call to load the next level after scrolling ends
            }
        }
    }

    // Method to trigger the LevelLoader to load the next level
    private void LoadNextLevel()
    {
        if (levelLoader != null)
        {
            levelLoader.LoadNextLevel(); // Trigger loading the next level
        }
        else
        {
            Debug.LogError("LevelLoader is not assigned!");
        }
    }
}
