using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject player;
    public static Camera mainCamera;
    internal static string ventFadeTag = "VentFade";

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;

        CameraStartingState();
    }

    public void CameraStartingState()
    {

    }

    internal static GameObject GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        return player;
    }
}
