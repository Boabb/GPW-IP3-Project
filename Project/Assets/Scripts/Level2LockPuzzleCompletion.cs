using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2LockPuzzleCompletion : MonoBehaviour
{
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private GameObject terka;
    [SerializeField] private GameObject brothers;

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
        AudioManager.PlayVoiceOverAudio(VoiceOverEnum.Level1Track2, 1.0f); // <- TO BE CHANGED!!!!

        // Spawn characters
        if (terka != null) terka.SetActive(true);
        if (brothers != null) brothers.SetActive(true);
    }
}
