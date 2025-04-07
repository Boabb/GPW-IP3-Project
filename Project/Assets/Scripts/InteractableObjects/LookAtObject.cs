using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    void onAwake()
    {
        Time.timeScale = 0f;
    }

    public void exit()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
