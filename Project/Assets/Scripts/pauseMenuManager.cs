using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static Transform inventoryButtonTransform;
    public GameObject[] screens; // Array of screens to switch between
    private int activeScreenIndex = -1; // Track active screen index
    public GameObject menuButton;
    public GameObject bg;

    private SettingsMenu settingsMenu; // Reference to settings script (if any)
    private GameObject controlScreen; // Reference to the control screen (if any)
    public int settingsScreenIndex = 1; // Index of your settings screen in 'screens'
    public int controlScreenIndex = 2; // Index of the control screen in 'screens'

    private void Awake()
    {
        inventoryButtonTransform = menuButton.transform;
    }
    void Start()
    {
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }

        if (screens.Length > settingsScreenIndex)
        {
            settingsMenu = screens[settingsScreenIndex].GetComponent<SettingsMenu>();
        }

        if (screens.Length > controlScreenIndex)
        {
            controlScreen = screens[controlScreenIndex]; // Set the reference to the control screen
        }

        bg.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleMenus();
        }
    }

    public void ToggleMenus()
    {
        if (activeScreenIndex != -1)
        {
            // If leaving settings or control screen without saving, revert them
            if ((activeScreenIndex == settingsScreenIndex || activeScreenIndex == controlScreenIndex) && settingsMenu != null)
            {
                settingsMenu.RevertSettings(); // Revert settings if we leave the settings menu without saving
            }

            // Deactivate all screens and reset the active screen
            foreach (GameObject screen in screens)
            {
                screen.SetActive(false);
            }

            activeScreenIndex = -1;
            bg.SetActive(false);
            Time.timeScale = 1;
            AudioListener.pause = false;

        }
        else
        {
            // Activate the main pause screen (0) and pause the game
            SwitchScreen(0);
            bg.SetActive(true);
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
    }

    public void SwitchScreen(int screenIndex)
    {
        if (screenIndex >= 0 && screenIndex < screens.Length)
        {
            // If switching away from settings or control screen without saving, revert
            if (activeScreenIndex == settingsScreenIndex && screenIndex != settingsScreenIndex && settingsMenu != null)
            {
                settingsMenu.RevertSettings(); // Revert the settings when switching away from settings or control screen
            }

            // Deactivate all screens and activate the target screen
            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].SetActive(i == screenIndex);
            }

            activeScreenIndex = screenIndex;
        }
    }

    public void MainMenu()
    {
        // Save settings if necessary before quitting to the main menu
        if (activeScreenIndex == settingsScreenIndex && settingsMenu != null)
        {
            settingsMenu.SaveChanges(); // Ensure changes are saved when leaving for the main menu
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void SettingsMenuToggle()
    {
        if (settingsMenu.gameObject.activeSelf == true) // Check if settings menu is active
        {
            settingsMenu.RevertSettings(); // Revert settings if the menu is being closed
        }

        settingsMenu.gameObject.SetActive(!settingsMenu.gameObject.activeSelf); // Toggle settings menu visibility
        screens[0].SetActive(!screens[0].activeSelf); // Toggle between the pause screen and settings screen
    }

    public void ControlScreenToggle()
    {
        if (controlScreen.gameObject.activeSelf == true) // Check if control screen is active
        {
            // If control screen is active and the user is switching, revert the settings
            if (settingsMenu != null)
            {
                settingsMenu.RevertSettings(); // Revert settings if leaving the control screen without saving
            }
        }

        // Toggle the control screen visibility
        controlScreen.gameObject.SetActive(!controlScreen.gameObject.activeSelf);
        screens[0].SetActive(!screens[0].activeSelf); // Toggle between the pause screen and control screen
    }
}