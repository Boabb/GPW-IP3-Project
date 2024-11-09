using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] GameObject agentToFollow;

    //where the camera is compared to the agent when it is 'directly' on the agent
    [SerializeField] float baseOffsetX = 0;
    [SerializeField] float baseOffsetY = 0;

    //the furthest the camera can move in a level regardless of offset
    [SerializeField] float levelMaxY;
    [SerializeField] float levelMinY;
    [SerializeField] float levelMaxX;
    [SerializeField] float levelMinX;

    //tigger objects
    GameObject[] triggerObjects;

    // Start is called before the first frame update
    void Start()
    {
        triggerObjects = GameObject.FindGameObjectsWithTag("FollowerTriggerObject");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(agentToFollow.transform.position.x, agentToFollow.transform.position.y, transform.position.z);
    }

    void ZoomToAgent()
    {

    }
}
