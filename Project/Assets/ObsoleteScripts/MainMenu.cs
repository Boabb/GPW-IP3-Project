using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;

    public void PlayGame()
    {
        //SceneManager.LoadScene("ControlScreen");
    }

    public void SettingsMenuToggle()
    {
        if(settingsMenu.gameObject.activeSelf == true)
        {
            settingsMenu.GetComponent<SettingsMenu>().RevertSettings();
        }
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

	private void Update()
	{
        // HACK: This only exists to turn the main menu back on after the settings screen is closed. Not the best way of doing this
        // but the quickest due to time constraints. What should happen is that the settings menu should fire off an event which is
        // is listened for in here, rather than poll whether its off each frame like below
		if (mainMenu.activeSelf == false && settingsMenu.gameObject.activeSelf == false)
        {
			mainMenu.SetActive(!mainMenu.activeSelf);
		}
	}
}
