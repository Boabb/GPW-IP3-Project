using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : InteractableObject
{
    [SerializeField] GameObject[] toActivate;
    public override void Interaction(GameObject playerGO)
    {
        foreach (GameObject obj in toActivate)
        {
            obj.SetActive(true);
        }
    }
}
