using UnityEngine;

public class SkyParallax : MonoBehaviour
{
    [SerializeField] private float parallaxEffectMultiplier = 0.5f;

    private Transform cam;
    private Vector3 lastCamPosition;

    private void Start()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCamPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier, 0);
        lastCamPosition = cam.position;
    }
}