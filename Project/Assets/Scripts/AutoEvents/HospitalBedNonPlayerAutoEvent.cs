using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalBedNonPlayerAutoEvent : MonoBehaviour
{
    [SerializeField] GameObject triggerObject;
    [SerializeField] GameObject objectToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == triggerObject)
        {
            objectToActivate.SetActive(true);
        }
        else
        {
            Debug.Log("Game Object: " + collision.gameObject);
        }
    }
}
