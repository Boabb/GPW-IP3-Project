using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : InteractableObject
{
    GameObject ClosedSprite;
    GameObject OpenSprite;
    Collider2D Collider;

    bool open;

    // Start is called before the first frame update
    void Start()
    {
		interactOnce = false;
		Transform[] transforms = GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject tempObject = transforms[i].gameObject;
            if (tempObject.name == "ClosedSprite")
            {
                ClosedSprite = tempObject;
            }

            if (tempObject.name == "OpenSprite")
            {
                OpenSprite = tempObject;
            }
        }

        Collider = gameObject.GetComponent<Collider2D>();

        if (ClosedSprite == null || OpenSprite == null || Collider == null)
        {
            Debug.LogWarning("A door interactable is incomplete");
        }
    }

    public override void Interaction(GameObject playerGO)
    {
        if(SystemSettings.GetPlayerActionPressed(SystemSettings.PlayerAction.Interact) == false)
        {
            return;
        }

        open = !open;

        if (open)
        {
            Collider.isTrigger = true;
            ClosedSprite.SetActive(false);
            OpenSprite.SetActive(true);
        }
        else
        {
            Collider.isTrigger = false;
            ClosedSprite.SetActive(true);
            OpenSprite.SetActive(false);
        }
    }
}
