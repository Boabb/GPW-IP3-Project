using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public GameObject[] items; // Array of collectible items
    private bool[] itemActive;  // Array to track if items are collected

    void Start()
    {
        // Optional: Uncomment to clear PlayerPrefs during testing
        PlayerPrefs.DeleteAll(); 

        itemActive = new bool[items.Length]; // Initialize the tracking array
        for (int i = 0; i < items.Length; i++)
        {
            // Get the collected state of each item
            int collectedState = PlayerPrefs.GetInt("ItemCollected_" + i, 0); // Default to 0 if key doesn't exist
            itemActive[i] = collectedState == 1; // true if collected
            items[i].SetActive(itemActive[i]); // Set item active if collected
            Debug.Log($"Item {i} collected state: {collectedState}, active: {itemActive[i]}");
        }
    }

    public void CollectItem(int itemIndex)
    {
        if (!itemActive[itemIndex]) // If the item has not been collected yet
        {
            itemActive[itemIndex] = true; // Mark the item as collected
            PlayerPrefs.SetInt("ItemCollected_" + itemIndex, 1); // Save the collected state
            items[itemIndex].SetActive(true); // Enable the item in the game
            Debug.Log($"Item {itemIndex} collected and saved.");
        }
        else
        {
            Debug.Log($"Item {itemIndex} already collected. Current state: {PlayerPrefs.GetInt("ItemCollected_" + itemIndex, 0)}");
        }
    }

    public void buttonPress()
    {
        AudioManager.PlayVoiceOverAudio(VoiceOverEnum.EmbroideredRoseCollectable, 5);
    }
}
