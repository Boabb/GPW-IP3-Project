using UnityEngine;
using TMPro; // For TextMeshPro
using UnityEngine.EventSystems;

public class LockPuzzle : MonoBehaviour
{
    public int[] currentCode = { 0, 0, 0, 0 }; // Player's current input
    public int[] correctCode = { 3, 5, 7, 2 }; // The correct code

    [SerializeField] private TMP_Text[] numberDisplays; // Assign in Inspector
    [SerializeField] private GameObject lockUI; // Assign in Inspector
    private bool isUIOpen = true;

    private void OnEnable()
    {
        OpenLockUI();
    }

    public void OpenLockUI()
    {
        lockUI.SetActive(true);
        isUIOpen = true;
        Time.timeScale = 0f; // Pause the game while interacting

        //Ensure the UI updates properly
        UpdateDisplay();

        Debug.Log("Lock UI opened, restoring previous values.");
    }


    public void CloseLockUI()
    {
        lockUI.SetActive(false);
        isUIOpen = false;
        Time.timeScale = 1f; // Resume the game
        Destroy(gameObject);
    }

    public void ExitLockUI()
    {
        lockUI.SetActive(false);
        isUIOpen = false;
        Time.timeScale = 1f;
    }


    // Function to change a number at a specific index
    public void ChangeNumber(int index, int change)
    {
        if (!isUIOpen) return; // Prevent changes when UI isn't open
        currentCode[index] = (currentCode[index] + change + 10) % 10; // Cycle between 0-9
        UpdateDisplay(); // Ensure UI updates

        Debug.Log($"Digit {index} changed to {currentCode[index]}");
    }

    // Wrapper functions for Unity UI Buttons
    public void IncreaseDigit(int index) { ChangeNumber(index, 1); }
    public void DecreaseDigit(int index) { ChangeNumber(index, -1); }

    private void UpdateDisplay()
    {
        for (int i = 0; i < numberDisplays.Length; i++)
        {
            if (numberDisplays[i] != null)
            {
                numberDisplays[i].text = currentCode[i].ToString();
                numberDisplays[i].ForceMeshUpdate(); // Force update UI in case timeScale = 0
            }
            else
            {
                Debug.LogError($"Number display at index {i} is not assigned in Inspector!");
            }
        }
    }

    public void CheckCode()
    {
        if (currentCode[0] == correctCode[0] &&
            currentCode[1] == correctCode[1] &&
            currentCode[2] == correctCode[2] &&
            currentCode[3] == correctCode[3])
        {
            Unlock();
        }
        else
        {
            Debug.Log("Incorrect Code");
        }
    }

    private void Unlock()
    {
        Debug.Log("Lock Opened!");
        CloseLockUI();
        // Add additional unlocking logic here if needed
    }
}
