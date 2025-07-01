using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : InteractableObject
{
    [SerializeField] GameObject[] toDeactivate;

    public override void Interaction(GameObject playerGO)
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
		// Delay by one game frame before disabling to give other objects a chance to do anything
		yield return null;

		foreach (var obj in toDeactivate)
		{
			obj.SetActive(false);
		}
	}
}
