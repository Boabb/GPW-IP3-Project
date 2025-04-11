using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextSentenceButton : MonoBehaviour
{
    public Button nextButton;
    public ContextScreen contextScreen;

    public float delayTime = 0.5f;

    public void NextSentence()
    {
        // Disable the button to prevent multiple clicks
        nextButton.interactable = false;

        // Check if SceneFlowManager is ready and not transitioning
        if (SceneFlowManager.Instance != null && !SceneFlowManager.Instance.isTransitioning)
        {
            // Call SceneFlowManager to show the next sentence
            SceneFlowManager.Instance.ShowNextSentence();
        }
        else
        {
            // Log if SceneFlowManager is transitioning or unavailable
            Debug.LogWarning("SceneFlowManager is either not found or transitioning!");
        }

        // Start the coroutine to re-enable the button after a delay
        StartCoroutine(ReenableButtonAfterDelay());
    }

    private IEnumerator ReenableButtonAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        // Re-enable the button after the delay
        nextButton.interactable = true;
    }
}
