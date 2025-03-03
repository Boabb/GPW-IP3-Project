using UnityEngine;
using System;

public class Collectables : InteractableObject
{
    public static event Action<int> OnItemCollected;

    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject collectableVisual;
    [SerializeField] private GameObject animated;
    private AudioSource collectableAudio;

    private bool canBeCollected = false;
    private GameObject currentPlayer;

    void Start()
    {
        collectableAudio = collectableVisual.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canBeCollected && currentPlayer != null && SystemSettings.interact)
        {
            Interaction(currentPlayer);
        }
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
        if (collectableVisual.activeSelf)
        {
            OnItemCollected?.Invoke(itemIndex);
            FindObjectOfType<PickupManager>().CollectItem(itemIndex);
            collectableVisual.SetActive(false);
            animated.SetActive(false);
        }
    }
}
