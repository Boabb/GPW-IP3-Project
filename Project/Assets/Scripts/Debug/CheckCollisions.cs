using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject + " OnTriggerEnter");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(gameObject + " OnTriggerExit");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(gameObject + " OnTriggerStay");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject + " OnCollisionEnter");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(gameObject + " OnCollisionExit");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(gameObject + " OnCollisionStay");
    }
}
