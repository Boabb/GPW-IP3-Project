using UnityEngine;

public class Interactions : MonoBehaviour
{
    //Rigidbody2D rb;
    public bool interact;
    Vector2 offset;
    Collider2D objectCollider;
    float playerY;
    LayerMask playerLayer;

    public enum InteractionType
    {
        moveWithPlayer,
    }

    public InteractionType interactionType; 
    
    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        interact = false;
        objectCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (!interact)
        {
            //objectCollider.isTrigger = false;
            objectCollider.excludeLayers = 0;
        }
        else
        {
            //objectCollider.isTrigger = true;
            objectCollider.excludeLayers = playerLayer;
        }
    }

    public void setOffset(Rigidbody2D player)
    {
        offset = new Vector2(player.position.x, player.position.y) - new Vector2(transform.position.x, transform.position.y);
        playerY = player.position.y;
    }

    public bool moveWithPlayer(Rigidbody2D player)
    {
        if (player.position.y > playerY)
        {
            interact = false;
            return false;
        }
        else
        {
            transform.position = new Vector2(player.position.x - offset.x, transform.position.y);
            return true;
        }
    }
}
