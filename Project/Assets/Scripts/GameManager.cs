using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static PlayerData playerData;
    public static Camera mainCamera;
    public static LevelLoader levelLoader => FindObjectOfType<LevelLoader>(true);
    public static PauseMenuManager pauseMenuManager => FindObjectOfType<PauseMenuManager>();
    internal static string ventFadeTag = "VentFade";

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        levelLoader.gameObject.SetActive(true);
    }
}
