using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypes : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;
    GameManager.MovementType movementType;
    float horizontalForce;

    Collider2D playerUprightCollider;
    Collider2D playerCrawlingCollider;

    private void Awake()
    {
        playerUprightCollider = GameObject.Find("UprightCollider").GetComponent<Collider2D>();
        playerCrawlingCollider = GameObject.Find("CrawlingCollider").GetComponent<Collider2D>();

        movementType = GameManager.MovementType.walk;
    }

    public void UpdateMovementType(ref Collider2D playerCollider, ref float movementSpeed, bool left)
    {
        playerCollider = UpdateCollider();
        movementSpeed = UpdateMovementSpeed(left);
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

    float UpdateMovementSpeed(bool left)
    {
        switch (movementType)
        {
            case GameManager.MovementType.walk:
                return gameManager.GetBaseWalkForce();
            case GameManager.MovementType.crawl:
                return gameManager.GetBaseCrawlForce();
            case GameManager.MovementType.pushPullLeft:
                if(left)
                {
                    return gameManager.GetBasePullForce();
                }
                else
                {
                    return gameManager.GetBasePushForce();
                }
            case GameManager.MovementType.pushPullRight:
                if (!left)
                {
                    return gameManager.GetBasePullForce();
                }
                else
                {
                    return gameManager.GetBasePushForce();
                }
            default:
                return gameManager.GetBaseWalkForce();
        }
    }
}
