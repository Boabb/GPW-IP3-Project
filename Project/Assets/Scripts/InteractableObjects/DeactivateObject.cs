using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : InteractableObject
{
    [SerializeField] GameObject[] toDeactivate;
    public override void Interaction(GameObject playerGO)
    {
        foreach (var obj in toDeactivate)
        {
            obj.SetActive(false);
        }
    }
}
