using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    GameObject player;
    bool interact;
    float offset;

    private void LateUpdate()
    {
        if (interact)
        {
            transform.position = new Vector3(player.transform.position.x - offset, transform.position.y, transform.position.z);
        }
        interact = false;
    }

    public void Interact(GameObject playerGO)
    {
        interact = true;
        player = playerGO;
        offset = player.transform.position.x - transform.position.x;
    }
}
