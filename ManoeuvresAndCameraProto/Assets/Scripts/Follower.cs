using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] GameObject agentToFollow;
    [SerializeField] float xOffset = 50;
    [SerializeField] float cameraSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        if (agentToFollow != null)
        {
            agentToFollow = GameManager.player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agentToFollow != null)
        {
            Vector3 position = new(agentToFollow.transform.position.x, agentToFollow.transform.position.y, transform.position.z);
            position.x -= xOffset;

            transform.position = Vector3.Lerp(transform.position, position, cameraSpeed);
        }
        else
        {
            agentToFollow = GameManager.player;
        }
    }
    
}
