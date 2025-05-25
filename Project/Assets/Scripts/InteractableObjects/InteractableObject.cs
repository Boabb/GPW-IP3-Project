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
    bool fixedInteract = false;
    private void Update()
    {
        interact = false;
        if (SystemSettings.tapInteract)
        {
            systemInteract = true;
        }
    }

    private void FixedUpdate()
    {
        if (systemInteract)
        {
            fixedInteract = true;
            systemInteract = false;
        }
        else
        {
            fixedInteract = false;
        }
    }

    public abstract void Interaction(GameObject playerGO);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (fixedInteract && collision.gameObject.layer == LayerMask.NameToLayer("Player") && !interact)
        {
            interact = true;
            Interaction(collision.gameObject);
        }

        systemInteract = false;
    }
}
