
using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] float interactRange = 500;
    bool interacting = false;
<<<<<<< Updated upstream

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
=======
    //// Update is called once per frame
    //void Update()
    //{
    //    if(Input.GetKey(KeyCode.E))
    //    {
    //        //GameObject playerRef = GameManager.player;
    //        //Ray r = new (playerRef.transform.position, playerRef.transform.up);
    //        //if(Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
    //        //{
    //        //    Debug.Log("1 step in!");
    //        //    if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactGameObject))
    //        //    {
    //        //        interactGameObject.Interact();
    //        //    }
    //        //}
    //    }
        
    //}
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Finally In");
        if (other.gameObject.TryGetComponent(out Interactable _))
        {
            if (!interacting && Input.GetKey(KeyCode.E))
            {
                interacting = true;
                StartCoroutine(Interact());
            }
        }
    }

    private void OnTriggerExit2D()
    {
        Debug.Log("Finally Out");
        interacting = false;
        StopCoroutine(Interact());
>>>>>>> Stashed changes
    }

    IEnumerator Interact()
    {
<<<<<<< Updated upstream


        while (interacting && Vector2.Distance(GameManager.player.transform.position, transform.position) <= interactRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interacted!");
            }
            yield return null;
        }
        yield return null;
=======
        while (interacting)
        {
            if (Vector2.Distance(GameManager.player.transform.position, transform.position) <= interactRange)
            {
                Debug.Log("Finally Interacted");
            }
            yield return null;
        }
>>>>>>> Stashed changes
    }
}
