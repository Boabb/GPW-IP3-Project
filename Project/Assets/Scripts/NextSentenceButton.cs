using UnityEngine;
using UnityEngine.UI;
using System.Collections; // For coroutines

public class NextSentenceButton : MonoBehaviour
{
    public Button nextButton;

    public void nextSentence()
    {
        // Disable the button to prevent further clicks
        nextButton.interactable = false;

        // Call the method to show the next sentence
        if (SceneFlowManager.Instance != null && !SceneFlowManager.Instance.isTransitioning)
        {
            SceneFlowManager.Instance.ShowNextSentence();
        }
        else
        {
            Debug.LogWarning("SceneFlowManager is either not found or transitioning!");
        }

        // Start the coroutine to re-enable the button after 2 seconds
        StartCoroutine(ReenableButtonAfterDelay());
    }

    private IEnumerator ReenableButtonAfterDelay()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Re-enable the button after the delay
        nextButton.interactable = true;
    }
}
