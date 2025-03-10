using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition; // Animator for the transition animation
    public float transitionTime = 1f; // Duration for the transition effect

    // This method can be triggered to load the next level
    public void LoadNextLevel()
    {
        // Start the transition and then load the level
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    // Coroutine to handle the transition effect and scene loading
    IEnumerator LoadLevel(int levelIndex)
    {
        // Trigger the transition animation
        transition.SetTrigger("Start");

        // Wait for the transition animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Now call the SceneFlowManager to load the next level
        // This will trigger the context screen and load the level after it's done
        SceneFlowManager.Instance.LoadNextLevel();
    }
}
