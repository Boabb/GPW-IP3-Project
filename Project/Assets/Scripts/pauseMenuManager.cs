using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject[] screens; // Array of screens to switch between
    private int activeScreenIndex = -1; // Track active screen index
    public GameObject menuButton;

    void Start()
    {
        // Loop through all screens and deactivate them
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
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
            // Hide all screens when closing the menu
            foreach (GameObject screen in screens)
            {
                screen.SetActive(false);
            }
            activeScreenIndex = -1;

            // Show the menu button again when the menus are closed
            menuButton.SetActive(true);
        }
        else
        {
            // Default to showing the first screen
            SwitchScreen(0);

            // Hide the menu button when the menus are opened
            menuButton.SetActive(false);
        }
    }


    public void SwitchScreen(int screenIndex)
    {
        if (screenIndex >= 0 && screenIndex < screens.Length)
        {
            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].SetActive(i == screenIndex); // Only activate the selected screen
            }
            activeScreenIndex = screenIndex;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
