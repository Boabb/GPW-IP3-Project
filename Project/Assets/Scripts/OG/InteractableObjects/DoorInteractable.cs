using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : InteractableObject
{
    GameObject UnwalkableTileRight;
    GameObject UnwalkableTileLeft;
    GameObject ClosedSprite;
    GameObject OpenSprite;

    bool open;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject tempObject = transforms[i].gameObject;

            if (tempObject.name == "UnwalkableTileRight")
            {
                UnwalkableTileRight = tempObject;
            }

            if (tempObject.name == "UnwalkableTileLeft")
            {
                UnwalkableTileLeft = tempObject;
            }

            if (tempObject.name == "ClosedSprite")
            {
                ClosedSprite = tempObject;
            }

            if (tempObject.name == "OpenSprite")
            {
                OpenSprite = tempObject;
            }
        }

        if (UnwalkableTileLeft == null || UnwalkableTileRight == null || ClosedSprite == null || OpenSprite == null)
        {
            Debug.LogWarning("A door interactable is incomplete");
        }
    }

    public override void Interaction(GameObject playerGO)
    {
        open = !open;

        if (open)
        {
            UnwalkableTileRight.SetActive(false);
            UnwalkableTileLeft.SetActive(false);
            ClosedSprite.SetActive(false);
            OpenSprite.SetActive(true);
        }
        else
        {
            UnwalkableTileRight.SetActive(true);
            UnwalkableTileLeft.SetActive(true);
            ClosedSprite.SetActive(true);
            OpenSprite.SetActive(false);
        }
    }
}
