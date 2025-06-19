using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : AutoEvent
{
    public static event Action<int> OnItemCollected;

    public Transform inventoryButton;

    private bool shouldBob = true;

    [SerializeField] private int itemIndex;
    [SerializeField] private float bobbingSpeed = 2f;
    [SerializeField] private float bobbingAmount = 0.1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        inventoryButton = PauseMenuManager.inventoryButtonTransform; 
    }

    void FixedUpdate()
    {
        if(shouldBob)
        {
            // Apply bobbing effect
            transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount, 0);
        }
    }

    public override void EventEnter(GameObject playerGO)
    {
        CollectItem(playerGO);
    }

    private void CollectItem(GameObject playerGO)
    {
        OnItemCollected?.Invoke(itemIndex);
        FindObjectOfType<PickupManager>().CollectItem(itemIndex);
        shouldBob = false;
        StartCoroutine(CollectingItem(this.gameObject));
    }

    private IEnumerator CollectingItem(GameObject gameObject)
    {
        float time = 0;
        while (Vector2.Distance(gameObject.transform.position, Camera.main.ScreenToWorldPoint(inventoryButton.position)) > .5f)
        {
            time += Time.deltaTime/12;
            gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, Camera.main.ScreenToWorldPoint(inventoryButton.position), time);
            gameObject.transform.localScale = Vector2.Scale(gameObject.transform.localScale, new(.99f, .99f));
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
