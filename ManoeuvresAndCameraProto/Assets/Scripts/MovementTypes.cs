using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypes : MonoBehaviour
{
    public GameManager gameManager;
    GameManager.MovementType movementType;
    float horizontalForce;

    Collider2D playerUprightCollider;
    Collider2D playerCrawlingCollider;

    private void Start()
    {
        playerUprightCollider = GameObject.Find("UprightCollider").GetComponent<Collider2D>();
        playerCrawlingCollider = GameObject.Find("CrawlingCollider").GetComponent<Collider2D>();
    }

    public void UpdateMovementType(ref Collider2D playerCollider)
    {
        playerCollider = UpdateCollider();
    }

    void Walk()
    {

    }
    void Crawl()
    {

    }
    void PushPullLeft() 
    {
    
    }
    void PushPullRight() 
    {
    
    }

    Collider2D UpdateCollider()
    {
        switch (movementType)
        {
            case GameManager.MovementType.walk:
                return playerUprightCollider;
            case GameManager.MovementType.crawl:
                return playerCrawlingCollider;
            case GameManager.MovementType.pushPullLeft:
                return playerUprightCollider;
            case GameManager.MovementType.pushPullRight:
                return playerUprightCollider;
            default:
                return playerUprightCollider;
        }
    }

    public void Move(float force)
    {
        horizontalForce = force;
        switch (movementType)
        {
            case GameManager.MovementType.walk:
                Walk(); 
                break;
            case GameManager.MovementType.crawl: 
                Crawl(); 
                break;
            case GameManager.MovementType.pushPullLeft: 
                PushPullLeft(); 
                break;
            case GameManager.MovementType.pushPullRight: 
                PushPullRight(); 
                break;
        }
    }
}
