using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{    
    public struct UnwalkableCoordinates
    {
        public float rightX, leftX, rightY, leftY;
    }
    public enum MovementType
    {
        walk,
        crawl,
        pushPullLeft,
        pushPullRight
    }

    public GameObject player;

    [Header("Movement Settings")]
    /// <summary> The horizontal force applied when walking. </summary> 
    [SerializeField] float BaseWalkForce = 10f;
    /// <summary> The horizontal force applied when crawling. </summary> 
    [SerializeField] float BaseCrawlForce = 5f;
    ///<summary> The horizontal force applied when pushing an object. </summary>
    [SerializeField] float BasePushForce = 6f;
    ///<summary> The horizontal force applied when pulling an object. </summary>
    [SerializeField] float BasePullForce = 4f;
    ///<summary> The vertical force applied when jumping. </summary>
    [SerializeField] float BaseJumpForce = 15f;

    [Header("Gravity Settings")]
    ///<summary> The force of gravity pulling the agent downwards </summary>
    [SerializeField] float GravityForce = 0.2f;
    ///<summary> The maximum speed the agent can fall at. </summary>
    float TerminalVelocity;

    // Start is called before the first frame update
    void Start()
    {
        TerminalVelocity = BaseJumpForce;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().gameManager = this;
    }

    public float GetBaseWalkForce()
    {
        return BaseWalkForce;
    }
    public float GetBaseCrawlForce()
    {
        return BaseCrawlForce;
    }
    public float GetBasePushForce()
    {
        return BasePushForce;
    }
    public float GetBasePullForce()
    {
        return BasePullForce;
    }
    public float GetBaseJumpForce()
    {
        return BaseJumpForce;
    }
    public float GetGravityForce()
    {
        return GravityForce;
    }
    public float GetTerminalVelocity()
    {
        return TerminalVelocity;
    }
}
