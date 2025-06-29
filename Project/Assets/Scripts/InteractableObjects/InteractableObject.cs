using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected GameObject player;
    protected bool interactedThisFrame;
    protected float offset;

    public abstract void Interaction(GameObject playerGO);

    private void OnTriggerStay2D(Collider2D collision)
    {
        interactedThisFrame = false;
        if (SystemSettings.interact && collision.gameObject.layer == LayerMask.NameToLayer("Player") && !interactedThisFrame)
        {
            interactedThisFrame = true;
            Interaction(collision.gameObject);
        }
    }
}
