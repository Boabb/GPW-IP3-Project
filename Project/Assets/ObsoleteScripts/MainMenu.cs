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
}
