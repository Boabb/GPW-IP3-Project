using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInteractable : InteractableObject
{
    [SerializeField] Vector3 position1;
    [SerializeField] Vector3 position2;

    float lerpProgress = 0f;
    [SerializeField] float lerpSpeed = 0.01f;
    bool up = false;
    bool move = false;
    bool movePlayer = false;
    Vector3 playerOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            PositionLerp();
        }

        if (player != null && player.transform.position.y < position2.y && transform.position == position2)
        {
            up = false;
            move = true;
        }
    }

    public override void Interaction(GameObject playerGO)
    {
        if (!move) //stops from changing direction halfway through move
        {
            player = playerGO;
            movePlayer = true;
            playerOffset = player.transform.position - transform.position;
            up = !up;
            move = true;
        }
    }

    void PositionLerp()
    {
        Vector3 lerp;
        if (up)
        { 
            lerp = Vector3.Lerp(position1, position2, lerpProgress);
            
        }
        else
        {
            lerp = Vector3.Lerp(position2, position1, lerpProgress);
        }

        gameObject.transform.position = lerp;

        if (movePlayer)
        {
            player.transform.position = lerp + playerOffset;
        }

        lerpProgress += lerpSpeed * Time.deltaTime;

        if (lerpProgress >= 1)
        {
            if (up)
            {
                gameObject.transform.position = position2;

                if (movePlayer)
                {
                    player.transform.position = position2 + playerOffset;
                }
            }
            else
            {
                gameObject.transform.position = position1; 
                
                if (movePlayer)
                {
                    player.transform.position = position1 + playerOffset;
                }
            }

            movePlayer = false;
            lerpProgress = 0;
            move = false;
        }
    }
}
