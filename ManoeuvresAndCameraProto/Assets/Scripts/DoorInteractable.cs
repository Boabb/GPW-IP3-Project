using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : InteractableObject
{
    GameObject UnwalkableTileRight;
    GameObject UnwalkableTileLeft;
    GameObject Sprite;

    bool open;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();

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

            if (tempObject.name == "Sprite")
            {
                Sprite = tempObject;
            }
        }

        if (UnwalkableTileLeft == null || UnwalkableTileRight == null || Sprite == null)
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
            Sprite.SetActive(false);
        }
        else
        {
            UnwalkableTileRight.SetActive(true);
            UnwalkableTileLeft.SetActive(true);
            Sprite.SetActive(true);
        }
    }
}
