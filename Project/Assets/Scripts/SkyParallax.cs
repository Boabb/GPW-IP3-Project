using UnityEngine;

public class SkyParallax : MonoBehaviour
{
    [SerializeField] private float parallaxEffectMultiplier = 0.5f;
    [SerializeField] bool camControl = true;

    private Transform cam;
    private Transform playerTransform;
    private Vector3 lastCamPosition;

    private void Start()
    {
        cam = Camera.main.transform;
        playerTransform = GameManager.playerData.gameObject.transform;
        lastCamPosition = cam.position;
    }

    private void LateUpdate()
    {      
        if (camControl)
        {
            Vector3 deltaMovement = cam.position - lastCamPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier, 0);
            lastCamPosition = cam.position;
        }
        else
        {
            Vector3 deltaMovement = GameManager.playerData.transform.position - lastCamPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, 0, 0);
            lastCamPosition = playerTransform.position;
        }
    }
}