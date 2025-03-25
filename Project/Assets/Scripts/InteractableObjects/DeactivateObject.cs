using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : InteractableObject
{
    [SerializeField] GameObject toDeactivate;
    public override void Interaction(GameObject playerGO)
    {
        toDeactivate.SetActive(false);
    }
}
