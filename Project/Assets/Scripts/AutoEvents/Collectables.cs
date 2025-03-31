using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : AutoEvent
{
    public static event Action<int> OnItemCollected;

    [SerializeField] private int itemIndex;
    [SerializeField] private float bobbingSpeed = 2f;
    [SerializeField] private float bobbingAmount = 0.1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Apply bobbing effect
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount, 0);
    }

    public override void EventEnter(GameObject playerGO)
    {
        CollectItem(playerGO);
    }

    private void CollectItem(GameObject playerGO)
    {
        OnItemCollected?.Invoke(itemIndex);
        FindObjectOfType<PickupManager>().CollectItem(itemIndex);
        Destroy(gameObject);
    }
}
