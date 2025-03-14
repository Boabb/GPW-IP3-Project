using UnityEngine;

public class CupboardInteraction : InteractableObject
{
    [SerializeField] private GameObject lockUI; // Assign in Inspector
    private bool canBeInteractedWith = false;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canBeInteractedWith = true;
            player = other.gameObject;
            Debug.Log("Player entered cupboard interaction range!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canBeInteractedWith = false;
            player = null;
            Debug.Log("Player left cupboard interaction range!");
        }
    }

    public override void Interaction(GameObject playerGO)
    {
        if (canBeInteractedWith)
        {
            Debug.Log("Cupboard Interacted! Opening Lock UI...");
            lockUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("Can't interact—player isn't close enough.");
        }
    }
}
