using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static PlayerData playerData;
    public static Camera mainCamera;
    public static CameraController cameraController;
    public static LevelLoader levelLoader => FindObjectOfType<LevelLoader>(true);
    public static PauseMenuManager pauseMenuManager => FindObjectOfType<PauseMenuManager>();
    internal static string ventFadeTag = "VentFade";
    private static GameObject lowerLevelSection;
    private static GameObject upperLevelSection;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
        if (playerData != null)
        {

        }
        else
        {
            //PlayerData.CallAwake();
        }
    }

    void Start()
    {
        if(lowerLevelSection == null)
        {
            lowerLevelSection = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.CompareTag("LowerLevelSection"));
        }        
        if(upperLevelSection == null)
        {
            upperLevelSection = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.CompareTag("UpperLevelSection"));
        }


        cameraController = GetComponent<CameraController>();
        levelLoader.gameObject.SetActive(true);
    }

    public static IEnumerator TransitionToOutsideSection(float elevatorSectionPeriod)
    {
        yield return new WaitForSeconds(elevatorSectionPeriod / 2);

        upperLevelSection.SetActive(false);
        lowerLevelSection.SetActive(true);

        yield return new WaitForSeconds(elevatorSectionPeriod / 2);

        //ElevatorCatchTrigger.DisableColliders = true;

    }
}
