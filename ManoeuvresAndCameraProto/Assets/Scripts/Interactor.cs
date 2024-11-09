
using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] float interactRange = 500;
    bool interacting = false;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Interactable")
        {
            interacting = true;
            StartCoroutine(Interact());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Interactable")
        {
            interacting = false;
            StopCoroutine(Interact());
        }
    }

    IEnumerator Interact()
    {


        while (interacting && Vector2.Distance(GameManager.player.transform.position, transform.position) <= interactRange)
        {
            if (SystemSettings.tapInteract)
            {
                Debug.Log("Interacted!");
                AudioManager.PlaySound(Resources.Load<AudioClip>("radiosound"));
            }
            yield return null;
        }
        yield return null;
    }
}
