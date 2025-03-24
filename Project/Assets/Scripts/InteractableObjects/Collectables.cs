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

    public VoiceOverEnum[] itemVoiceOvers;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            currentPlayer = other.gameObject;
            canBeCollected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            currentPlayer = null;
            canBeCollected = false;
        }
    }

    public override void Interaction(GameObject playerGO)
    {
        OnItemCollected?.Invoke(itemIndex);
        FindObjectOfType<PickupManager>().CollectItem(itemIndex);

        // Play the correct voice-over based on item index
        if (itemIndex >= 0 && itemIndex < itemVoiceOvers.Length)
        {
            AudioManager.PlayVoiceOverAudio(itemVoiceOvers[itemIndex]);
        }
        else
        {
            Debug.LogWarning("Invalid item index for voice-over playback.");
        }

        Destroy(gameObject);
    }
}
