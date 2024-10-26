using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 playerSize;
    Rigidbody2D playerRb;

    int groundLayer;
    int playerLayer;

    public int maxSpeed;
    public int jumpPower;
    public int climbSpeed;
    public int uprightSpeed;
    public int crawlSpeed;
    public float groundCheckRadius;

    public bool interacting;

    int speed;
    [SerializeField] Collider2D jumpSpace; //from inspector

    Vector2 groundNormal;
    float groundPosition;
    bool grounded;

    bool canJump;
    bool hasCaught;

    float caughtHeight;
    float playerTempCaughtHeight;

    enum MovementMode
    {
        upright,
        crawl
    }

    MovementMode movementMode = MovementMode.upright;

    void Start()
    {
        speed = uprightSpeed;
        canJump = true;

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");
        playerSize = GetComponent<Collider2D>().bounds.size;
        playerRb = GetComponent<Rigidbody2D>();
        jumpSpace.excludeLayers = playerLayer;
    }

    void Update()
    {
    
    }

    private void FixedUpdate()
    {
        if (SystemSettings.interact)
        {
            interacting = true;
        }
        else
        {
            interacting = false;
        }

        checkGround();
        checkMovementType();

        move();
        jump();
        jumpAndCatch();
        climb();
    }

    void checkMovementType()
    {
        //https://discussions.unity.com/t/raycast-layermask-parameter/802897/7
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - (playerSize.x / 2) - 0.05f, transform.position.y + (playerSize.y / 2)), transform.right, 0.1f + playerSize.x, ~playerLayer);
        RaycastHit2D notClear = Physics2D.Raycast(new Vector2(transform.position.x - (playerSize.x / 2) - 0.05f, transform.position.y - (playerSize.y / 4)), transform.right, 0.1f + playerSize.x, ~playerLayer);

        RaycastHit2D roof = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + playerSize.x), new Vector2 (playerSize.y, playerSize.x), 0, Vector2.zero, 0, ~playerLayer);

        if (!roof && movementMode == MovementMode.crawl)
        {
            movementMode = MovementMode.upright;
            speed = uprightSpeed;
            canJump = true;
            playerRb.SetRotation(0);
        }
        
        
        if (!interacting && hit && !notClear && movementMode == MovementMode.upright)
        {
            movementMode = MovementMode.crawl;
            speed = crawlSpeed;
            canJump = false;
            playerRb.velocity = Vector2.zero;
            
            playerRb.SetRotation(90);
        }

        if (movementMode == MovementMode.upright)
        {
        }
        else if (movementMode == MovementMode.crawl)
        {
        }
    }

    void checkGround()
    {
        float playerModifer;
        Vector2 rayDirection;
        if (movementMode == MovementMode.crawl)
        {
            playerModifer = playerSize.x / 2;
            rayDirection = -transform.right;
        }
        else
        {
            playerModifer = playerSize.y / 2;
            rayDirection = -transform.up;
        }

        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, rayDirection, Mathf.Infinity, groundLayer);
        if (hit)
        {
            groundPosition = transform.position.y - hit.distance + playerModifer;
            groundNormal = hit.normal;
        }

        if (transform.position.y > groundPosition - groundCheckRadius && transform.position.y < groundPosition + groundCheckRadius)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void move()
    {
        if (hasCaught) //climb caught object
        {
            playerRb.AddForce(new Vector2(0, climbSpeed));

            if (transform.position.y - (playerSize.y / 2) > caughtHeight)
            {
                hasCaught = false;
                canJump = true;
            }
        }
        else if (SystemSettings.moveRight && !SystemSettings.moveLeft) //walk right
        {
            float currentSpeed = groundNormal.x + speed;
            checkMaxSpeed(currentSpeed);

            playerRb.AddForce(new Vector2(currentSpeed, groundNormal.y));
        }
        else if (SystemSettings.moveLeft && !SystemSettings.moveRight) //walk left
        {
            float currentSpeed = groundNormal.x + -speed;
            checkMaxSpeed(currentSpeed);

            playerRb.AddForce(new Vector2(currentSpeed, groundNormal.y));
        }

        void checkMaxSpeed(float currentSpeed)
        {
            if (currentSpeed < -maxSpeed || currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;

            }
        }
    }

    void jump()
    {
        //if (jumpSpace.IsTouchingLayers(Physics2D.AllLayers))
        //{
        //    canJump = false;
        //}

        if (hasCaught || interacting)
        {
            canJump = false;
        }

        if (canJump && grounded && SystemSettings.jump)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
            playerRb.AddForce(new Vector2(0, jumpPower));
        }
    }

    void jumpAndCatch()
    {
        RaycastHit2D clear = Physics2D.Raycast(new Vector2(transform.position.x - (playerSize.x / 2) - 0.05f, transform.position.y + ((playerSize.y / 2) + (playerSize.y / 4))), transform.right, 0.1f + playerSize.x, ~playerLayer);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - (playerSize.x / 2) - 0.05f, transform.position.y + (playerSize.y / 4)), transform.right, 0.1f + playerSize.x, ~playerLayer);

        if (!interacting && !hasCaught && !grounded && hit && !clear)
        {
            hasCaught = true;
            canJump = false;

            caughtHeight = hit.collider.bounds.max.y;
            playerTempCaughtHeight = hit.collider.bounds.max.y - (playerSize.y / 2);

            transform.position = new Vector2(transform.position.x, playerTempCaughtHeight);
        }
    }

    void climb()
    {

    }

    float angularLerp(float lerp1, float lerp2, ref float currentProgress, float animSpeed)
    {
        currentProgress += animSpeed;
        return Mathf.LerpAngle(lerp1, lerp2, currentProgress);
    }

    float positionLerp(float lerp1, float lerp2, ref float currentProgress, float animSpeed)
    {
        currentProgress += animSpeed;
        return Mathf.Lerp(lerp1, lerp2, currentProgress);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
