using UnityEngine;
using System;

public class Collectables : InteractableObject
{
    public static event Action<int> OnItemCollected;

    [SerializeField] private int itemIndex;
    [SerializeField] private float bobbingSpeed = 2f;
    [SerializeField] private float bobbingAmount = 0.1f;

    private bool canBeCollected = false;
    private GameObject currentPlayer;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (canBeCollected && currentPlayer != null && SystemSettings.interact)
        {
            Interaction(currentPlayer);
        }

        // Apply bobbing effect
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.gameObject;
            canBeCollected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = null;
            canBeCollected = false;
        }
    }

    public override void Interaction(GameObject playerGO)
    {
        OnItemCollected?.Invoke(itemIndex);
        FindObjectOfType<PickupManager>().CollectItem(itemIndex);
        AudioManager.PlayVoiceOverAudio(VoiceOverEnum.EmbroideredRoseCollectable);
        Destroy(gameObject); // Removes the collectable from the scene
    }
}
