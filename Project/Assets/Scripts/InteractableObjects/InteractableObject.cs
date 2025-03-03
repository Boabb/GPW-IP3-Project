using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected GameObject player;
    protected bool interact;
    protected float offset;

    bool systemInteract = false;
    private void Update()
    {
        interact = false;
        if (SystemSettings.tapInteract)
        {
            systemInteract = true;
        }
        Debug.Log("Tap Interact Update: " + systemInteract);
    }

    private void FixedUpdate()
    {
        Debug.Log("Tap Interact Fixed Update: " + systemInteract);
    }

    public abstract void Interaction(GameObject playerGO);

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Tap Interact Trigger: " + systemInteract);
        if (systemInteract && collision.gameObject.tag == "Player" && !interact)
        {
            interact = true;
            Interaction(collision.gameObject);
        }

        systemInteract = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        systemInteract = false;
    }
}
