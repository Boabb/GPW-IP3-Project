using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected GameObject player;
    protected bool interact;
    protected float offset;

    protected virtual void LateUpdate() { }

    public abstract void Interaction(GameObject playerGO);
}
