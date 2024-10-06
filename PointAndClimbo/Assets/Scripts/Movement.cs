using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //constants
    [SerializeField] int baseSpeed = 10;
    [SerializeField] int baseJump = 5;
    [SerializeField] float maxAcceleration = 3;
    [SerializeField] float minAcceleration = 1;
    [SerializeField] float accelerationAlterer = 1;

    //variables for checking grounded
    bool grounded = true;
    RaycastHit2D ground;
    float groundY;
    [SerializeField] Vector2 boxSize;
    [SerializeField] float castDistance;
    [SerializeField] LayerMask groundLayer;

    //variables for jumping
    bool isJumping;
    float jumpOrigin;
    float jumpProgress;
    float jumpHeight;
    float acceleration;

    //variables for falling
    bool isFalling;
    float fallOrigin;
    float fallProgress;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectGround();

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            return; 
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(-baseSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(baseSpeed);
        }

        if (!isJumping && grounded && Input.GetKey(KeyCode.Space))
        {
            BeginJump();
        }

        if (isJumping)
        {
            Jump();
        }

        if (!grounded && !isJumping && !isFalling)
        {
            BeginFall(minAcceleration);
        }

        if (isFalling)
        {
            Fall();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            DebugInfo();
        }
    }

    void Move(int speed)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + (speed * Time.deltaTime) , gameObject.transform.position.y, gameObject.transform.position.z);
    }

    void BeginJump()
    {
        grounded = false;
        isJumping = true;
        jumpOrigin = gameObject.transform.position.y;
        jumpProgress = 0;
        jumpHeight = gameObject.transform.position.y + baseJump;
        acceleration = maxAcceleration;

        Jump();
    }

    void Jump()
    {
        if (grounded)
        {
            isJumping = false;
            jumpProgress = 0;
        }
        else
        {
            float jumpSpeed = acceleration * Time.deltaTime;
            jumpProgress += jumpSpeed;
            gameObject.transform.position = Vector3.Lerp(new Vector3(gameObject.transform.position.x, jumpOrigin, 0), new Vector3(gameObject.transform.position.x, jumpOrigin + jumpHeight), jumpProgress);
            acceleration = acceleration / accelerationAlterer;

            if (acceleration < minAcceleration)
            {
                acceleration = minAcceleration;
            }

            if (jumpProgress >= 1)
            {
                BeginFall(acceleration);
            }
        }
    }

    void BeginFall(float originalAcceleration)
    {
        grounded = false;
        isJumping = false;
        isFalling = true;
        fallOrigin = gameObject.transform.position.y;
        fallProgress = 0;
        acceleration = originalAcceleration;

        Fall();
    }

    void Fall()
    {
        float fallSpeed = acceleration * Time.deltaTime;
        fallProgress += fallSpeed;
        gameObject.transform.position = Vector3.Lerp(new Vector3(gameObject.transform.position.x, fallOrigin, 0), new Vector3(gameObject.transform.position.x, groundY), fallProgress);
        acceleration = acceleration * accelerationAlterer;

        if (acceleration > maxAcceleration)
        {
            acceleration = maxAcceleration;
        }

        if (fallProgress >= 1)
        {
            isFalling = false;
            fallProgress = 0;
        }

        if (grounded)
        {
            isFalling = false;
            fallProgress = 0;
        }
    }

    //https://www.youtube.com/watch?v=P_6W-36QfLA
    void DetectGround()
    {
        ground = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);
        groundY = transform.position.y - ground.distance;
        if (transform.position.y <= groundY)
        {
            transform.position = new Vector2 (transform.position.x, groundY);
            grounded = true;
            isJumping = false;
            isFalling = false;
        }
        else
        {
            grounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    void DebugInfo()
    {
        Debug.Log("Grounded: " + grounded);
        Debug.Log("Is Jumping: " + isJumping);
        Debug.Log("Is Falling: " + isFalling);
        Debug.Log("GroundY: " + groundY);
    }
}
