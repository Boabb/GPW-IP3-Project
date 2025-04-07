using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2LockPuzzleCompletion : MonoBehaviour
{
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectsToDeactivate;
    [SerializeField] private VoiceOverEnum voiceOverToPlay;

    private void OnEnable()
    {
        puzzleManager.OnPuzzleCompleted += HandlePuzzleCompletion;
    }

    private void OnDisable()
    {
        puzzleManager.OnPuzzleCompleted -= HandlePuzzleCompletion;
    }

    private void HandlePuzzleCompletion()
    {
        Debug.Log("The door is unlocked. Triggers everything now.");

        // Start next testimony
        AudioManager.PlayVoiceOverAudio(voiceOverToPlay, 1.0f);

        // Spawn characters
        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].SetActive(true);
        }

        for (int i = 0;i < objectsToDeactivate.Length; i++)
        {
            objectsToDeactivate[i].SetActive(false);
        }
    }
}
