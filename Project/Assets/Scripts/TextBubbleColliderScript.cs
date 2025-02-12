using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubbleColliderScript : MonoBehaviour
{
    public string inputText = "Hello, Player!";  // Default text
    private TextBubbleScript TBS;

    void Start()
    {
        TBS = GameObject.Find("TextBubbleUIObject")?.GetComponent<TextBubbleScript>();

        if (TBS == null)
        {
            Debug.LogError("TextBubbleScript not found! Make sure a GameObject named 'TextBubble' exists.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure correct tag
        {
            Debug.Log("Player entered trigger! Sending text...");
            TBS?.SetText(inputText);

            gameObject.SetActive(false);
        }
    }
}
