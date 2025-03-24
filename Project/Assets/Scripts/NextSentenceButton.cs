using UnityEngine;
using UnityEngine.UI;

public class NextSentenceButton : MonoBehaviour
{

    public void nextSentence()
    {
        if (SceneFlowManager.Instance != null)
        {
            SceneFlowManager.Instance.ShowNextSentence();
        }
        else
        {
            Debug.LogError("SceneFlowManager not found in the scene!");
        }
    }
}
