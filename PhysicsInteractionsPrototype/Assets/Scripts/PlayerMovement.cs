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

    int speed;
    [SerializeField] Collider2D jumpSpace; //from inspector

    Vector2 groundNormal;
    float groundPosition;
    bool grounded;

    bool canJump;

    enum MovementMode
    {
        upright,
        crawl
    }

    MovementMode movementMode = MovementMode.upright;

    void Start()
    {
        speed = uprightSpeed;
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
        checkGround();
        checkMovementType();

        walk();
        jump();
        climb();
    }

    void checkMovementType()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + (playerSize.x / 2), transform.position.y + (playerSize.y / 2)), transform.right, 0.05f);

        Debug.Log(hit.collider);

        if (hit && movementMode == MovementMode.upright)
        {
            movementMode = MovementMode.crawl;
            speed = crawlSpeed;
            canJump = false;
            playerRb.velocity = Vector2.zero;
            playerRb.SetRotation(90);
        }
        else if (!hit && movementMode == MovementMode.crawl)
        {
            movementMode = MovementMode.upright;
            speed = uprightSpeed;
            canJump = true;
            playerRb.SetRotation(0);
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
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, -transform.up, Mathf.Infinity, groundLayer);
        if (hit)
        {
            groundPosition = transform.position.y - hit.distance + (playerSize.y / 2);
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

    void walk()
    {
        //walk
        if (SystemSettings.moveRight && !SystemSettings.moveLeft)
        {
            float currentSpeed = groundNormal.x + speed;
            checkMaxSpeed(currentSpeed);

            playerRb.AddForce(new Vector2(currentSpeed, groundNormal.y));
        }
        else if (SystemSettings.moveLeft && !SystemSettings.moveRight)
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
        if (jumpSpace.IsTouchingLayers(Physics2D.AllLayers))
        {
            canJump = false;
        }

        if (canJump && grounded && SystemSettings.jump)
        {
            Debug.Log("Jump");
            playerRb.AddForce(new Vector2(0, jumpPower));
        }
    }

    void jumpAndCatch()
    {

    }

    void climb()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
