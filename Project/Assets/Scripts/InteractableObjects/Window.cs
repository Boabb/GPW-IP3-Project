using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour
{
    public float openAngle = 45f;  // How much the window should rotate
    public float speed = 2f;       // Speed of rotation
    private bool isOpen = false;   // Prevent re-triggering

    [SerializeField] ObjectTags doorTags;  // Reference to the DOOR'S ObjectTags
    private Collider hitbox;

    private void Start()
    {
        hitbox = GetComponentInChildren<Collider>();
    }

    public void OpenWindow()
    {
        if (!isOpen)
        {
            isOpen = true;

            if (doorTags != null)
            {
                doorTags.clingClimbable = false;
            }

            StartCoroutine(RotateWindow());
        }
    }

    private IEnumerator RotateWindow()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - openAngle);

        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime * speed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time);
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure exact final position

        if (doorTags != null)
        {
            doorTags.clingClimbable = true;
        }

        // Disable the child hitbox after falling
        if (hitbox != null)
        {
            hitbox.enabled = false;
        }
    }
}
