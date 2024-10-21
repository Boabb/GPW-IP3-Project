using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] float speedDivider = 10;
    Vector3 worldClickPosition;
    bool move;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            worldClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            move = true;
        }

        if (move)
        {
            gameObject.transform.position = gameObject.transform.position - (new Vector3(worldClickPosition.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 0) / speedDivider);
            worldClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            move = false;
        }

        Debug.Log(worldClickPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
