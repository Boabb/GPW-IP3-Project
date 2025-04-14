using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettings : MonoBehaviour
{
    public static bool moveLeft;
    public static bool moveRight;
    public static bool tapLeft;
    public static bool tapRight;
    public static bool jump;
    public static bool interact;
    public static bool tapInteract;

    [SerializeField] Camera mainCamera;

    [SerializeField] GameObject touchControlsParent; //this should be activated if touch controls are in use

    enum SystemType
    {
        TouchScreen,
        Desktop
    }

    SystemType systemType = SystemType.Desktop;

    private void Start()
    {
        string system = SystemInfo.operatingSystem;

        if (system.Contains("Windows") || system.Contains("Mac"))
        {
            systemType = SystemType.Desktop;
        }
        else if (system.Contains("Android") || system.Contains("OS"))
        {
            systemType = SystemType.TouchScreen;
        }
        else
        {
            Debug.LogWarning("Can't detect system type... Reverting to Default");
            systemType = SystemType.Desktop;
        }

//#if UNITY_EDITOR
//        systemType = SystemType.TouchScreen;
//#endif

        if (systemType == SystemType.TouchScreen)
        {
            if (touchControlsParent != null)
                touchControlsParent.SetActive(true);
        }
        else
        {
            if (touchControlsParent != null)
                touchControlsParent.SetActive(false);
        }
    }

    private void Update()
    {
        if (systemType == SystemType.Desktop)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveLeft = true;
            }
            else
            {
                moveLeft = false;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveRight = true;
            }
            else
            {
                moveRight = false;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                jump = true;
            }
            else
            {
                jump = false;
            }

            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.LeftShift))
            {
                interact = true;
            }
            else
            {
                interact = false;
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                tapInteract = true;
            }
            else
            {
                tapInteract = false;
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                tapLeft = true;
            }
            else
            {
                tapLeft = false;
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                tapRight = true;
            }
            else
            {
                tapRight = false;
            }
        }

        tapInteract = false;
    }

    public void OnLeftDown()
    {
        print("left pressed");
        SystemSettings.moveLeft = true;
        Debug.Log("moveLeft: " + SystemSettings.moveLeft); // Debugging
    }

    public void OnLeftUp()
    {
        print("left unpressed");
        SystemSettings.moveLeft = false;
        Debug.Log("moveLeft: " + SystemSettings.moveLeft); // Debugging
    }

    public void OnRightDown()
    {
        print("Right pressed");
        SystemSettings.moveRight = true;
        Debug.Log("moveRight: " + SystemSettings.moveRight); // Debugging
    }

    public void OnRightUp()
    {
        print("Right unpressed");
        SystemSettings.moveRight = false;
        Debug.Log("moveRight: " + SystemSettings.moveRight); // Debugging
    }

    public void OnJumpDown()
    {
        jump = true;  // Trigger jump when jump button is pressed
        Debug.Log("Jump: " + jump);  // Debugging
    }

    public void OnJumpUp()
    {
        jump = false;  // Stop jump when jump button is released
        Debug.Log("Jump: " + jump);  // Debugging
    }

    public void OnInteractDown() { interact = true; tapInteract = true; }
    public void OnInteractUp() { interact = false;}
}