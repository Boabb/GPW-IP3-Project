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

    [SerializeField] Camera mainCamera;

    [SerializeField] GameObject touchControlsParent; //this should be activated if touch controls are in use

    //these are used to control the placement of the controls depending on resolution of the screen
    [SerializeField] GameObject movementParent;
    [SerializeField] GameObject otherParent;

    //the actual buttons to be clicked on
    [SerializeField] Collider2D rightButton;
    [SerializeField] Collider2D leftButton;
    [SerializeField] Collider2D jumpButton;
    [SerializeField] Collider2D interactButton;

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

        if (systemType == SystemType.TouchScreen)
        {
            touchControlsParent.SetActive(true);
        }
    }

    private void Update()
    {
        float viewHeight = mainCamera.orthographicSize;
        float viewWidth = viewHeight * mainCamera.aspect;

        Vector3 movementButtons = new Vector3(-viewWidth + (viewWidth / 4) + mainCamera.transform.position.x, -viewHeight + (viewHeight / 3) + mainCamera.transform.position.y, 0);
        Vector3 otherButtons = new Vector3(viewWidth - (viewWidth / 4) + mainCamera.transform.position.x, -viewHeight + (viewHeight / 3) + mainCamera.transform.position.y, 0);

        movementParent.transform.position = movementButtons;
        otherParent.transform.position = otherButtons;

        //Debug.Log(movementParent.transform.position);
        //Debug.Log(otherParent.transform.position);

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

            if (Input.GetKey(KeyCode.Space))
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

        if (systemType == SystemType.TouchScreen)
        {

            moveLeft = false;
            moveRight = false;
            interact = false;
            jump = false;
            tapLeft = false;
            tapRight = false;
            tapInteract = false;

            Ray[] rays = new Ray[Input.touchCount];
            for (int i = 0; i < Input.touchCount; i++)
            {
                rays[i] = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                Collider2D checkCollider = Physics2D.Raycast(rays[i].origin, rays[i].direction).collider;

                if (checkCollider == leftButton && (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Stationary))
                {
                    moveLeft = true;
                }

                if (checkCollider == rightButton && (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Stationary))
                {
                    moveRight = true;
                }

                if (checkCollider == interactButton && (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Stationary))
                {
                    interact = true;
                }

                if (checkCollider == jumpButton && Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    jump = true;
                }

                if (checkCollider == leftButton && Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    tapLeft = true;
                }

                if (checkCollider == rightButton && Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    tapRight = true;
                }

                if (checkCollider == interactButton && Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    tapInteract = true;
                }
            }
        }
    }
}
