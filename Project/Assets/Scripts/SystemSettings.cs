using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettings : MonoBehaviour
{
	[SerializeField] Camera mainCamera;

	[SerializeField] GameObject touchControlsParent; //this should be activated if touch controls are in use

	public enum PlayerAction
    {
        MoveLeft = 0,
        MoveRight = 1,
		Jump = 2,
		Interact = 3,
	}

	enum SystemType
    {
        TouchScreen,
        Desktop
    }

    SystemType systemType = SystemType.Desktop;

	/// <summary>Stores which player actions are currently on. Uses the individual bits in the variable to determine this according to the bit locations that PlayerAction represents</summary>
	private static int playerActionsOn;

	/// <summary
    /// Stores which player actions are currently pressed, therefore only when the first frame is on.
    /// Uses the individual bits in the variable to determine this according to the bit locations that PlayerAction represents
    /// </summary>
	private static int playerActionsPressed;

	public static bool GetPlayerActionOn(PlayerAction action)
	{
		return (playerActionsOn & (1 << (int)action)) > 0;
	}
	public static bool GetPlayerActionPressed(PlayerAction action)
	{
		return (playerActionsPressed & (1 << (int)action)) > 0;
	}
	private static int GetPlayerActionMask(PlayerAction action)
	{
		return 1 << (int)action;
	}

	private void Start()
    {
        string system = SystemInfo.operatingSystem;

        if (system.Contains("Windows") || system.Contains("Mac") || system.Contains("Linux"))
        {
            systemType = SystemType.Desktop;
        }
        else if (system.Contains("Android") || system.Contains("iOS") || system.Contains("iPad") || system.Contains("iPhone"))
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
        // Store last frames values, will need this for being able to test if a key is pressed
        var previousPlayerActionsOn = playerActionsOn;

        // Reset values and recalculate each frame
        playerActionsOn = 0;
		playerActionsPressed = 0;

		if (systemType == SystemType.Desktop)
        {
            // Cache values to make it easier to read
            const int moveLeftAsInt = (int)PlayerAction.MoveLeft;
			const int moveRightAsInt = (int)PlayerAction.MoveRight;
			const int JumpAsInt = (int)PlayerAction.Jump;
			const int InteractAsInt = (int)PlayerAction.Interact;
			int moveLeftMask = GetPlayerActionMask(PlayerAction.MoveLeft);
			int moveRightMask = GetPlayerActionMask(PlayerAction.MoveRight);
			int jumpMask = GetPlayerActionMask(PlayerAction.Jump);
			int interactMask = GetPlayerActionMask(PlayerAction.Interact);

			// For each action convert active keys to an int of 0 or 1 representing if active. Then bitshift the 0 or 1 value by the number of places
			// defined by the PlayerAction so that the corresponding bit in playerActionsOn is set. This can be retrieved later
			playerActionsOn |= (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1 : 0) << moveLeftAsInt;
			playerActionsOn |= (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? 1 : 0) << moveRightAsInt;
            playerActionsOn |= (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0) << JumpAsInt;
            playerActionsOn |= (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 1 : 0) << InteractAsInt;

			// Need to check if previously a PlayerAction was off and that it is on now to set that PlayerAction as being pressed. First a bitwise mask is used
			// to only get the bit of the PlayerAction we want to check in previousPlayerActionsOn. Only if this is 0, off, do we check if it is on in playerActionsOn
			// using the same mask. Only if both conditions are true do we then set the corresponding bit in playerActionsOn by bitshifting the 0 or 1 value
            // by the number of places defined by the PlayerAction
			playerActionsPressed |= (((previousPlayerActionsOn & moveLeftMask) == 0 && (playerActionsOn & moveLeftMask) > 0) ? 1 : 0) << moveLeftAsInt;
			playerActionsPressed |= (((previousPlayerActionsOn & moveRightMask) == 0 && (playerActionsOn & moveRightMask) > 0) ? 1 : 0) << moveRightAsInt;
			playerActionsPressed |= (((previousPlayerActionsOn & jumpMask) == 0 && (playerActionsOn & jumpMask) > 0) ? 1 : 0) << JumpAsInt;
			playerActionsPressed |= (((previousPlayerActionsOn & interactMask) == 0 && (playerActionsOn & interactMask) > 0) ? 1 : 0) << InteractAsInt;
        }
    }
}