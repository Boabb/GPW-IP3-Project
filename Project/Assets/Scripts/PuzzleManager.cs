using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : InteractableObject
{
    [SerializeField] private GameObject puzzleUI; // Assign the UI in Inspector

    public delegate void PuzzleCompletedEvent();
    public event PuzzleCompletedEvent OnPuzzleCompleted;

    public Sprite openLocker;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private PuzzleCompletionAction completionAction;

    // Reference to the cabinet GameObject (optional for certain objects)
    [SerializeField] private GameObject cabinet;
    public float minClearDistance = 0.5f;

    public enum PuzzleCompletionAction
    {
        ClosePuzzleUI,
        OpenLocker
    }

    private void Start()
    {
        interactOnce = false;
        spriteRenderer = GetComponent<SpriteRenderer>();

        OnPuzzleCompleted += HandlePuzzleCompletion;
    }

    public override void Interaction(GameObject playerGO)
    {
		// Only open or close puzzle UI when the interact button is released
		if (!SystemSettings.GetPlayerActionPressed(SystemSettings.PlayerAction.Interact))
		{
            return;
		}

		if (player != null && (!cabinet || !IsCabinetBlocking())) // Check cabinet only if it's assigned
        {
            TogglePuzzleUI();  // Toggle UI open/closed
        }
        else if (player != null)
        {
            Debug.Log($"{gameObject.name}: Cabinet is blocking interaction!");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Can't interact—player isn't close enough.");
        }
    }

    private void TogglePuzzleUI()
    {
        // Toggle the UI visibility
        if (puzzleUI != null)
        {
            bool isUIActive = puzzleUI.activeSelf;

            // Toggle the UI state
            puzzleUI.SetActive(!isUIActive);

            // Pause or resume the game based on the UI state
            if (!isUIActive)
            {
                Time.timeScale = 0f; // Pause the game when UI is opened
                Debug.Log($"{gameObject.name}: Puzzle UI opened.");
            }
            else
            {
                Time.timeScale = 1f; // Resume the game when UI is closed
                Debug.Log($"{gameObject.name}: Puzzle UI closed.");
            }
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: No UI assigned for this puzzle.");
        }
    }

    private bool IsCabinetBlocking()
    {
        // Check if the cabinet is too close to the puzzle, preventing interaction
        return Vector2.Distance(cabinet.transform.position, transform.position) <= minClearDistance;
    }

    public void CompletePuzzle()
    {
        Debug.Log($"{gameObject.name}: Puzzle completed!");

        OnPuzzleCompleted?.Invoke();
    }

    private void HandlePuzzleCompletion()
    {
        switch (completionAction)
        {
            case PuzzleCompletionAction.ClosePuzzleUI:
                ClosePuzzleUI();
                break;
            case PuzzleCompletionAction.OpenLocker:
                OpenLocker();
                ClosePuzzleUI();
                break;
            default:
                Debug.Log($"{gameObject.name}: No special action on completion.");
                break;
        }
    }

    private void ClosePuzzleUI()
    {
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(false);
            Time.timeScale = 1f; // Resume the game
            Debug.Log($"{gameObject.name}: Puzzle UI closed.");
        }
    }

    private void OpenLocker()
    {
        spriteRenderer.sprite = openLocker;
    }
}
