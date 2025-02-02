using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableInteractable : InteractableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void LateUpdate()
    {
        if (interact)
        {
            transform.position = new Vector3(player.transform.position.x - offset, transform.position.y, transform.position.z);
        }
        interact = false;
    }

    public override void Interaction(GameObject playerGO)
    {
        interact = true;
        player = playerGO;
        offset = player.transform.position.x - transform.position.x;
    }
}
