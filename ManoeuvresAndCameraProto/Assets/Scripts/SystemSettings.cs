using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettings: MonoBehaviour
{
    public static bool moveLeft;
    public static bool moveRight;
    public static bool tapLeft;
    public static bool tapRight;
    public static bool jump;
    public static bool interact;
    public static bool tapInteract;

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
    }

    private void Update()
    {
        if (systemType == SystemType.Desktop)
        {
            if (Input.GetKey(KeyCode.A))
            {
                moveLeft = true;
            }
            else
            {
                moveLeft = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                moveRight = true;
            }
            else
            {
                moveRight = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump = true;
            }
            else
            {
                jump = false;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                interact = true;
            }
            else
            {
                interact = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                tapInteract = true;
            }
            else
            {
                tapInteract = false;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                tapLeft = true;
            }
            else
            {
                tapLeft = false;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                tapRight = true;
            }
            else
            {
                tapRight = false;
            }
        }
    }
}
