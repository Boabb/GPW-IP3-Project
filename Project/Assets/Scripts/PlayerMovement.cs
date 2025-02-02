using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerData playerData;
    Collider2D uprightCollider;
    Collider2D crawlingCollider;
    Rigidbody2D playerRB2D;

    Collider2D currentPlayerCollider;
    MovementType movementType;

    [Header("For Designers")]
    [SerializeField] float walkingForce = 10f;
    [SerializeField] float crawlingForce = 5f;

    float movementForce;
    enum MovementType
    {
        Walking,
        Crawling,
        Pulling
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = GetComponent<PlayerData>();
        uprightCollider = playerData.playerWalkingCollider;
        crawlingCollider = playerData.playerCrawlingCollider;
        playerRB2D = playerData.playerRigidbody;

        currentPlayerCollider = uprightCollider;
        playerRB2D = GetComponent<Rigidbody2D>();
        movementType = MovementType.Walking;
        movementForce = walkingForce;
    }

    // Update is called once per frame
    void Update()
    {
        //temp
        movementForce = walkingForce;
        //end temp
        UpdateMovementType();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Debug.Log("Player mass: " + playerRB2D.mass);
        if (movementType != MovementType.Pulling)
        {
            if (SystemSettings.moveLeft && !SystemSettings.moveRight)
            {
                playerRB2D.AddForce(-transform.right * movementForce);
            }

            if (SystemSettings.moveRight && !SystemSettings.moveLeft)
            {
                playerRB2D.AddForce(transform.right * movementForce);
            }
        }
    }

    void UpdateMovementType()
    {
        //implement this!
        //should switch between walking and crawling!

        if (playerData.pulling)
        {
            movementType = MovementType.Pulling;
        }
        else
        {
            //temp
            movementType = MovementType.Walking;
        }
    }

    public float GetMovementForce()
    {
        return movementForce;
    }
}
