using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMovement : MonoBehaviour
{
    struct PathSection
    {
        public Vector2 target;
        public GameObject marker;
        public bool jump;
    }

    [Header("Gravity Settings")]
    /// <summary> The horizontal force applied when moving. </summary> 
    [SerializeField] int BaseHorizontalSpeed = 10;
    ///<summary> The vertical force applied when jumping. </summary>
    [SerializeField] int BaseVerticalSpeed = 15;
    ///<summary> The force of gravity pulling the agent downwards </summary>
    [SerializeField] float GravityForce = 0.2f; 
    /// <summary> //the force which slows the agent while it is in the air </summary>
    [SerializeField] float HorizontalAirResistance = 0.05f;
    ///<summary> The margin in which the agent can land. </summary>
    [SerializeField] float GroundedMargin = 0.03f;
    ///<summary> The margin in which the agent can reach a target. </summary>
    [SerializeField] float TargetMargin = 0.1f; 

    [Header("Pathfinding Variables")]

    ///<summary> The target set to path towards as the mouse pointer. </summary>
    PathSection target; 
    List<PathSection> subTargets = new List<PathSection>();
    [SerializeField] Vector2 horizontalBoxSize;
    [SerializeField] LayerMask obstacleLayer;
    PathSection currentTarget;
    int currentTargetNo;
    bool followPath;

    ///player central boarders
    Vector3 rightEdgePosition;
    Vector3 leftEdgePosition;
    Vector3 topEdgePosition;
    Vector3 bottomEdgePosition;

    ///variables for checking grounded
    bool grounded; 
    RaycastHit2D ground;
    float groundY = -3.99f;
    [SerializeField] Vector2 verticalBoxSize;
    [SerializeField] float castDistance = 20;
    /// <summary> The Layer within the Unity Editor that will be the 'ground'. </summary>
    [SerializeField] LayerMask groundLayer;
    bool isJumping;

    /// Movement variables
    float horizontalSpeed;
    float verticalSpeed;

    [Header("Touchscreen Controls")]
    public GameObject JoyStick;
    Transform JoyStickAnchor;
    float JoyStickRadius;

    [Header("Debug")]
    public GameObject endTargetPrefab;
    public GameObject subTargetPrefab;

    /// Control schemes
    enum ControlScheme
    {
        Keyboard,
        PointAndClick,
        TouchScreen
    }

    ControlScheme Control = ControlScheme.TouchScreen;

    private void Start()
    {
        target.target = transform.position;
        JoyStickAnchor = JoyStick.transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Control)
        {
            case ControlScheme.Keyboard:
                KeyControl();
                break;
            case ControlScheme.PointAndClick:
                PointAndClickControl();
                break;
            case ControlScheme.TouchScreen:
                TouchControl();
                break;
        }

        if (verticalSpeed > 0)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        if (verticalSpeed < -15)
        {
            verticalSpeed = -15;
        }

        Movement(ref horizontalSpeed, ref verticalSpeed, HorizontalAirResistance, GravityForce);
        DetectGround();
    }

    void TouchControl()
    {
        for (int c = 0; c < Input.touchCount; c++)
        {
            if (c >= 2)
            {
                break;
            }

            if (Input.GetTouch(c).position.x < Screen.width / 2) //move
            {
                JoyStick.transform.position = Input.GetTouch(c).position;

                if (Input.GetTouch(c).position.x < Screen.width / 8)
                {
                    horizontalSpeed = -BaseHorizontalSpeed;
                }
                else
                {
                    horizontalSpeed = BaseHorizontalSpeed; 
                }            
            }
            else if (grounded && Input.GetTouch(c).phase == TouchPhase.Ended) //jump
            {
                verticalSpeed = BaseVerticalSpeed;
            }
        }
    }

    void KeyControl()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            return;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalSpeed = -BaseHorizontalSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalSpeed = BaseHorizontalSpeed;
        }

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalSpeed = BaseVerticalSpeed;
        }
    }

    void PointAndClickControl()
    {
        if (Input.GetMouseButtonDown(0)) //if left click
        {
            followPath = true;
            GetTarget();
            FindPath();
        }

        MoveAlongPath();
    }

    void GetTarget()
    {
        Vector3 tempTarget = GetMousePosition();
        RaycastHit2D mouseRay = Physics2D.Raycast(tempTarget, -transform.up, 100, groundLayer);
        target.target = new Vector2 (mouseRay.collider.ClosestPoint(tempTarget).x, mouseRay.collider.ClosestPoint(tempTarget).y + transform.localScale.y / 2);
    }

    void FindPath()
    {
        float subTargetOffset;
        foreach (PathSection subTarget in subTargets)
        {
            Destroy(subTarget.marker);
        }
        Destroy(target.marker);

        subTargets.Clear();
        RaycastHit2D[] obstacles;

        if (target.target.x > transform.position.x)
        {
            subTargetOffset = 1;
            obstacles = Physics2D.BoxCastAll(transform.position, horizontalBoxSize, 0, transform.right, castDistance, obstacleLayer);
        }
        else
        {
            subTargetOffset = -1;
            obstacles = Physics2D.BoxCastAll(transform.position, horizontalBoxSize, 0, -transform.right, castDistance, obstacleLayer);
        }

        PathSection firstTarget = new PathSection();
        firstTarget.target = transform.position;

        subTargets.Add(firstTarget);

        foreach (RaycastHit2D obstacle in obstacles)
        {
            PathSection pathSection1 = new PathSection();
            PathSection pathSection2 = new PathSection();

            if (obstacle.collider.bounds.size.y < 1.5f) //if not too high
            {
                if (Vector2.Distance(transform.position, obstacle.collider.ClosestPoint(transform.position)) < Vector2.Distance(transform.position, target.target))
                {
                    pathSection1.target = new Vector2(obstacle.collider.ClosestPoint(subTargets[subTargets.Count - 1].target).x - subTargetOffset, obstacle.collider.ClosestPoint(subTargets[subTargets.Count - 1].target).y);
                    pathSection1.marker = Instantiate(subTargetPrefab);
                    pathSection1.marker.transform.position = pathSection1.target;
                    subTargets.Add(pathSection1);

                    if (Vector2.Distance(transform.position, new Vector2(obstacle.collider.ClosestPoint(transform.position).x + (subTargetOffset * 3), obstacle.collider.transform.position.y)) < Vector2.Distance(transform.position, target.target))
                    {
                        pathSection2.target = new Vector2(obstacle.collider.ClosestPoint(transform.position).x + subTargetOffset, obstacle.collider.transform.position.y + obstacle.collider.transform.localScale.y / 2 + transform.localScale.y / 2);
                        pathSection2.marker = Instantiate(subTargetPrefab);
                        pathSection2.marker.transform.position = pathSection2.target;
                        pathSection2.jump = true;
                        subTargets.Add(pathSection2);
                    }
                }
            }
            else
            {
                currentTargetNo = 0;
                followPath = false;

                Debug.LogWarning("Path Failed!");
                return;
            }
        }

        target.marker = Instantiate(endTargetPrefab);
        target.marker.transform.position = target.target;

        currentTargetNo = 0;

        if (subTargets.Count == 0)
        {
            currentTarget = target; 
            currentTargetNo = -1;
        }
        else
        {
            currentTarget = subTargets[currentTargetNo];
        }

        //Debug.Log("Current Target: " + currentTargetNo);
    }

    void MoveAlongPath()
    {
        if (followPath)
        {
            if (currentTarget.target.y - transform.position.y > TargetMargin && grounded)
            {
                verticalSpeed = BaseVerticalSpeed;
            }

            if (currentTarget.target.x > transform.position.x)
            {
                horizontalSpeed = BaseHorizontalSpeed;
            }
            else
            {
                horizontalSpeed = -BaseHorizontalSpeed;
            }

            if (Vector2.Distance(transform.position, target.target) < TargetMargin)
            {
                currentTargetNo = 0;
                followPath = false;

                Debug.Log("Finished Path!");
            }
            else if (Vector2.Distance(transform.position, currentTarget.target) < TargetMargin || currentTarget.jump == true && groundY - currentTarget.target.y < TargetMargin && transform.position.x - currentTarget.target.x < TargetMargin)
            {
                if (currentTarget.jump == true && groundY - currentTarget.target.y < TargetMargin && transform.position.x - currentTarget.target.x < TargetMargin)
                {
                    Debug.Log("Jump Target");
                    Debug.Log("Ground Y: " + groundY);
                    Debug.Log("Target Y: " +  currentTarget.target.y);
                }

                if (Vector2.Distance(transform.position, currentTarget.target) < TargetMargin)
                {
                    Debug.Log("Not Jump Target");
                    Debug.Log("Ground Y: " + groundY);
                    Debug.Log("Target Y: " + currentTarget.target.y);
                }

                Destroy(currentTarget.marker);
                currentTargetNo++;

                if (currentTargetNo >= subTargets.Count)
                {
                    currentTarget = target;
                    currentTargetNo = -1;
                }
                else
                {
                    currentTarget = subTargets[currentTargetNo];
                }

                //Debug.Log("Current Target: " + currentTargetNo);
            }
        }        
    }

    void Movement(ref float xSpeed, ref float ySpeed, float xAntiForce, float yAntiForce)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + (xSpeed * Time.deltaTime), gameObject.transform.position.y + (ySpeed * Time.deltaTime), gameObject.transform.position.z);

    }

    //https://www.youtube.com/watch?v=P_6W-36QfLA
    void DetectGround()
    {
        SetEdgePositions();
        RaycastHit2D checkGround = Physics2D.BoxCast(bottomEdgePosition, verticalBoxSize, 0, -transform.up, castDistance, groundLayer);
        float checkGroundY;

        try
        {
            checkGroundY = checkGround.collider.ClosestPoint(transform.position).y;
        }
        catch
        {
            checkGroundY = -int.MaxValue;
        }

        if (Vector2.Distance(bottomEdgePosition, new Vector2(transform.position.x, checkGroundY)) < GroundedMargin && !isJumping)
        {
            transform.position = new Vector3(transform.position.x, groundY + transform.localScale.y / 2, 0);
            grounded = true;
            verticalSpeed = 0;
            horizontalSpeed = 0;
        }
        else
        {
            grounded = false;
            groundY = checkGroundY;
            ground = checkGround;
            verticalSpeed -= GravityForce; 
            
            if (horizontalSpeed < 0f)
            {
                horizontalSpeed += HorizontalAirResistance;

                if (horizontalSpeed > 0f)
                {
                    horizontalSpeed = 0;
                }
            }
            else if (horizontalSpeed > 0f)
            {
                horizontalSpeed -= HorizontalAirResistance;

                if (horizontalSpeed < 0f)
                {
                    horizontalSpeed = 0;
                }
            }
        }
    }

    void SetEdgePositions()
    {
        rightEdgePosition = new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y, transform.position.z);
        leftEdgePosition = new Vector3(transform.position.x - transform.localScale.x / 2, transform.position.y, transform.position.z);
        topEdgePosition = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
        bottomEdgePosition = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(bottomEdgePosition - transform.up * castDistance, verticalBoxSize);
    }

    public void ChangeToKeyboard()
    {
        ChangeFromPointAndClick();
        Control = ControlScheme.Keyboard;
    }
    public void ChangeToTouchScreen()
    {
        ChangeFromPointAndClick();
        Control = ControlScheme.TouchScreen;
    }
    public void ChangeToPointAndClick()
    {
        Control = ControlScheme.PointAndClick;
    }

    void ChangeFromPointAndClick()
    {
        followPath = false; 
        foreach (PathSection subTarget in subTargets)
        {
            Destroy(subTarget.marker);
        }
        Destroy(target.marker);

        subTargets.Clear();
    }

    //from Hamid Homatash
    Vector3 GetMousePosition()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(temp.x, temp.y, 0.0f);
    }
}
