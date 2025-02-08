using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject inventoryScreen;
    public GameObject controlScreen;

    void Start()
    {
        Time.timeScale = 1f;
        inventoryScreen.SetActive(false);
        controlScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            bool isAnyScreenActive = inventoryScreen.activeSelf || controlScreen.activeSelf;

            // If any screen is active, deactivate both
            inventoryScreen.SetActive(!isAnyScreenActive);
            controlScreen.SetActive(false);
        }
    }

    public void SwitchScreen()
    {
        bool inventoryActive = inventoryScreen.activeSelf;

        // Ensure both screens never show at the same time
        inventoryScreen.SetActive(!inventoryActive);
        controlScreen.SetActive(inventoryActive);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
