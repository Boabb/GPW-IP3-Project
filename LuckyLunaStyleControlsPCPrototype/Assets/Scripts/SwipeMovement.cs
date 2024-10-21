using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10;
    [SerializeField] float speed = 10;
    [SerializeField] float drag = 10;
    float currentSpeed = 0;
    Vector3 clickPosition;
    Rigidbody2D rb;

    bool updateMovement;
    bool pauseMovementTillNextSwipe;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pauseMovementTillNextSwipe = false;
            clickPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if ((clickPosition.x - Input.mousePosition.x > 1 || clickPosition.x - Input.mousePosition.x < -1) && !pauseMovementTillNextSwipe)
            {
                updateMovement = true;
                pauseMovementTillNextSwipe = true;
            }
        }

        if (updateMovement)
        {
            beginMove(clickPosition.x - Input.mousePosition.x);
            updateMovement = false;
        }
        else
        {
            continuousMove();
        }
    }

    void beginMove(float direction)
    {
        bool positive = true;

        if (direction < 0)
        {
            positive = false;
            direction = direction * -1;
        }

        currentSpeed = direction * Time.deltaTime;

        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

        if (!positive)
        {
            currentSpeed = -currentSpeed;
        }

        Debug.Log(currentSpeed);

        gameObject.transform.position = gameObject.transform.position + new Vector3(currentSpeed, 0);
    }

    void continuousMove()
    {
        if (currentSpeed > -0.001 && currentSpeed < 0.001)
        {
            currentSpeed = 0;
        }

        if (currentSpeed < 0)
        {
            currentSpeed += drag * Time.deltaTime;
        }
        else if (currentSpeed > 0)
        {
            currentSpeed -= drag * Time.deltaTime;
        }
        gameObject.transform.position = gameObject.transform.position + new Vector3(currentSpeed, 0);
    }
}
