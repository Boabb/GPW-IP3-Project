using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentBombAutoEvent : AutoEvent
{
    private void Start()
    {
        
    }

    public override void EventEnter(GameObject playerGO)
    {
        StartCoroutine(StartShake());
    }

    IEnumerator StartShake()
    {
        yield return new WaitForSeconds(5f);
        GameManager.cameraController.StartShake(1f, 0.5f, 0.5f);
    }
}
