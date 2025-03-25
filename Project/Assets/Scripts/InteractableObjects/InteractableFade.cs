using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFade : InteractableObject
{
    FadeOutAutoEvent fadeOut;

    public override void Interaction(GameObject playerGO)
    {
        fadeOut = GetComponentInChildren<FadeOutAutoEvent>();
        fadeOut.EventEnter(playerGO);
    }
}
