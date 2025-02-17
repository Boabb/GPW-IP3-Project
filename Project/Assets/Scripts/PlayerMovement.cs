using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerData playerData;
    Collider2D uprightCollider;
    Collider2D crawlingCollider;
    //Collider2D groundedCollider;
    Rigidbody2D playerRB2D;

    Collider2D currentPlayerCollider;
    MovementType movementType;

    [Header("For Designers")]
    [SerializeField] float walkingForce = 100f;
    [SerializeField] float crawlingForce = 50f;
    [SerializeField] float jumpForce = 1000f;

    [SerializeField] float gravityForce = 100f;
    [SerializeField] float dragForce = 100f;

    float pushForce;
    float pullForce;

    float movementForce;
    [HideInInspector] public bool grounded;
    enum MovementType
    {
        Walking,
        Crawling,
        Pulling,
        Pushing
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = GetComponent<PlayerData>();
        uprightCollider = playerData.playerWalkingCollider;
        crawlingCollider = playerData.playerCrawlingCollider;
        //groundedCollider = playerData.playerGroundedCollider;
        playerRB2D = playerData.playerRigidbody;

        currentPlayerCollider = uprightCollider;
        playerRB2D = GetComponent<Rigidbody2D>();
        movementType = MovementType.Walking;
        movementForce = walkingForce;

        pushForce = walkingForce / 2;
        pullForce = walkingForce / 3;
    }

    // Update is called once per frame
    void Update()
    {
        //temp
        movementForce = walkingForce;
        //end temp
        UpdateMovementType();
        UpdateMovementForce();
        SetGrounded();

        Debug.Log("Ground: " + grounded);
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        ApplyGravity();
        ApplyHorizontalDrag();
    }

    void Move()
    {
        //Debug.Log("Player mass: " + playerRB2D.mass);
        if (movementType != MovementType.Pulling)
        {
            if (SystemSettings.moveLeft && !SystemSettings.moveRight)
            {
                playerRB2D.velocity = new Vector3(-transform.right.x * movementForce * Time.fixedDeltaTime, playerRB2D.velocity.y, 0);
            }

            if (SystemSettings.moveRight && !SystemSettings.moveLeft)
            {
                playerRB2D.velocity = new Vector3(transform.right.x * movementForce * Time.fixedDeltaTime, playerRB2D.velocity.y, 0);
            }
        }
        AudioManager.StopSoundEffect(SoundEffect.WoodenFootsteps);
    }

    void Jump()
    {
        SetGrounded();
        if (SystemSettings.jump && grounded == true)
        {
            AudioManager.PlaySoundEffect(SoundEffect.SuzanneExert);
            Debug.Log("Jump");
            playerRB2D.velocity = new Vector3(playerRB2D.velocity.x, transform.up.y * jumpForce * Time.fixedDeltaTime, 0);
        }
    }

    void UpdateMovementType()
    {
        //implement this!
        //should switch between walking and crawling!

        if (playerData.pulling)
        {
            movementType = MovementType.Pulling;
            AudioManager.PlaySoundEffect(SoundEffect.WoodenFootsteps);
        }
        else if (playerData.pushing)
        {
            movementType = MovementType.Pushing;
        }
        else
        {
            //temp
            movementType = MovementType.Walking;
        }
    }

    void UpdateMovementForce()
    {
        if (movementType == MovementType.Pulling)
        {
            movementForce = pullForce;
        }
        else if (movementType == MovementType.Pushing)
        {
            movementForce = pushForce;
        }
        else if (movementType == MovementType.Crawling)
        {
            movementForce = crawlingForce;
        }
        else //default to walking
        {
            movementForce = walkingForce;
        }
    }

    void ApplyGravity()
    {
        playerRB2D.velocity = new Vector3(playerRB2D.velocity.x, playerRB2D.velocity.y - (gravityForce * Time.fixedDeltaTime), 0);
    }

    void ApplyHorizontalDrag()
    {
        float dragValue = dragForce * Time.fixedDeltaTime;
        //Debug.Log("Player X velocity: " + playerRB2D.velocity.x);

        if (playerRB2D.velocity.x != 0) //the player is moving on the x axis
        {
            if (playerRB2D.velocity.x > 0) //positive movement
            {
                if (playerRB2D.velocity.x - dragValue < 0)
                {
                    //Debug.Log("positive should 0");
                    playerRB2D.velocity = new Vector3(0, playerRB2D.velocity.y, 0);
                }
                else
                {
                    //Debug.Log("positive");
                    playerRB2D.velocity = new Vector3(playerRB2D.velocity.x - (dragForce * Time.fixedDeltaTime), playerRB2D.velocity.y, 0);
                }
            }
            else //negative movement
            {
                if (playerRB2D.velocity.x + dragValue > 0)
                {
                    //Debug.Log("negative should 0");
                    playerRB2D.velocity = new Vector3(0, playerRB2D.velocity.y, 0);
                }
                else
                {
                    //Debug.Log("negative");
                    playerRB2D.velocity = new Vector3(playerRB2D.velocity.x + (dragForce * Time.fixedDeltaTime), playerRB2D.velocity.y, 0);
                }
            }
        }

        //playerRB2D.velocity = new Vector3(playerRB2D.velocity.x - (dragForce * Time.fixedDeltaTime), playerRB2D.velocity.y, 0);
    }

    void SetGrounded()
    {
        if (playerRB2D.velocity.y == 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    public float GetMovementForce()
    {
        return movementForce;
    }
}
