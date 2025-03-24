using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupManager : MonoBehaviour
{
    public GameObject[] items;
    public Material grayscaleMaterial;
    private Material defaultMaterial;
    private bool[] itemActive;

    public VoiceOverEnum[] itemVoiceOvers;

    void Start()
    {
        // Optional: Uncomment to clear PlayerPrefs during testing
        PlayerPrefs.DeleteAll(); 

        itemActive = new bool[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            int collectedState = PlayerPrefs.GetInt("ItemCollected_" + i, 0);
            itemActive[i] = collectedState == 1;

            
            items[i].SetActive(true);

            Image imageComponent = items[i].GetComponentInChildren<Image>();
            Button buttonComponent = items[i].GetComponentInChildren<Button>(); // Find the Button component

            if (imageComponent != null)
            {
                // Store the default material only once
                if (defaultMaterial == null)
                {
                    defaultMaterial = imageComponent.material;
                }

                // Apply the appropriate material based on collected state
                imageComponent.material = itemActive[i] ? defaultMaterial : grayscaleMaterial;
            }
            else
            {
                Debug.LogWarning($"No Image component found on {items[i].name} or its children!");
            }

            // Enable or disable button interaction based on collected state
            if (buttonComponent != null)
            {
                buttonComponent.interactable = itemActive[i];
            }
            else
            {
                Debug.LogWarning($"No Button component found on {items[i].name} or its children!");
            }
        }
    }

    public void CollectItem(int itemIndex)
    {
        if (!itemActive[itemIndex]) // If the item has not been collected yet
        {
            itemActive[itemIndex] = true; // Mark the item as collected
            PlayerPrefs.SetInt("ItemCollected_" + itemIndex, 1); // Save state
            PlayerPrefs.Save(); // Ensure data is written immediately

            Image imageComponent = items[itemIndex].GetComponentInChildren<Image>();
            Button buttonComponent = items[itemIndex].GetComponentInChildren<Button>();

            if (imageComponent != null)
            {
                imageComponent.material = defaultMaterial;
            }

            if (buttonComponent != null)
            {
                buttonComponent.interactable = true;
            }
        }
    }

    public void ButtonPress(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < itemVoiceOvers.Length)
        {
            AudioManager.PlayVoiceOverAudio(itemVoiceOvers[itemIndex], 5);
        }
        else
        {
            Debug.LogWarning("Invalid item index or voice-over not assigned.");
        }
    }
}
