using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject[] screens; // Array of screens to switch between
    private int activeScreenIndex = -1; // Track active screen index
    public GameObject menuButton;
    public GameObject bg;

    private SettingsMenu settingsMenu; // Reference to settings script (if any)
    public int settingsScreenIndex = 1; // Index of your settings screen in 'screens'

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

        bg.SetActive(false);
        Time.timeScale = 1;
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
            // If leaving settings without saving, revert them
            if (activeScreenIndex == settingsScreenIndex && settingsMenu != null)
            {
                settingsMenu.RevertSettings();
            }

            foreach (GameObject screen in screens)
            {
                screen.SetActive(false);
            }
            activeScreenIndex = -1;

            menuButton.SetActive(true);
            bg.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            SwitchScreen(0);

            menuButton.SetActive(false);
            bg.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void SwitchScreen(int screenIndex)
    {
        if (screenIndex >= 0 && screenIndex < screens.Length)
        {
            // If switching away from settings without saving, revert
            if (activeScreenIndex == settingsScreenIndex && screenIndex != settingsScreenIndex && settingsMenu != null)
            {
                settingsMenu.RevertSettings();
            }

            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].SetActive(i == screenIndex);
            }
            activeScreenIndex = screenIndex;
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}