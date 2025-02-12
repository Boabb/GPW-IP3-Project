using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerData playerData;
    Collider2D uprightCollider;
    Collider2D crawlingCollider;
    Collider2D groundedCollider;
    Rigidbody2D playerRB2D;

    Collider2D currentPlayerCollider;
    MovementType movementType;

    [Header("For Designers")]
    [SerializeField] float walkingForce = 10f;
    [SerializeField] float crawlingForce = 5f;
    [SerializeField] float jumpForce = 100f;

    float movementForce;
    [HideInInspector] public bool grounded;
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
        groundedCollider = playerData.playerGroundedCollider;
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

        //Debug.Log("Ground: " + grounded);
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        //Debug.Log("Player mass: " + playerRB2D.mass);
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

    void Jump()
    {
        SetGrounded();
        if (SystemSettings.jump && grounded == true)
        {
            Debug.Log("Jump");
            playerRB2D.AddForce(transform.up * jumpForce);
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

    void SetGrounded()
    {
        List<Collider2D> overlappingColliders = new List<Collider2D>();

        groundedCollider.OverlapCollider(new ContactFilter2D().NoFilter(), overlappingColliders);

        for (int i = 0; i < overlappingColliders.Count; i++)
        {
            if (overlappingColliders[i].tag == "Ground")
            {
                grounded = true;
                break;
            }
            else
            {
                grounded = false;
            }
        }

        //test
        if (playerRB2D.velocity.y == 0)
        {
            grounded = true;
        }
    }

    public float GetMovementForce()
    {
        return movementForce;
    }
}
