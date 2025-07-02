using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHospitalEvent : AutoEvent
{
    PlayerData playerData;

    int stage = 0;
    bool stageActive = false;

    float stage3Count = 5f;
    float stage3Subtractor = 0.5f;
    bool stage3End = false;
    [SerializeField] FadeInAutoEvent fadeIn;
    [SerializeField] FadeOutAutoEvent fadeOut;
    CameraController camCon;

    [SerializeField] GameObject[] familyRenderers;
    [SerializeField] GameObject hospitalRenderer;
    [SerializeField] GameObject[] deactivateObjects;

    private void Start()
    {
        playerData = GameManager.playerData;
        camCon = GameManager.mainCamera.GetComponent<CameraController>();
    }

    private void Update()
    {
        switch (stage)
        {
            case 0:
                break;
            case 1:
                if (!stageActive)
                {
                    playerData.FreezePlayer(); //freeze player
                    AudioManager.PlayVoiceOverWithSubtitles(VoiceOverEnum.Level2Track4); //testimony begins
                    camCon.LerpToZoom(2f, 2); //zoom on family and door

                    for (int i = 0; i < familyRenderers.Length; i++)
                    {
                        //familyRenderers[i].transform.localScale = new Vector3(0.08f, 0.08f, 1);
                    }

                    stageActive = true;
                }

                if (!camCon.GetZoomStatus())
                {
                    stage = 2;
                    stageActive = false;
                }
                break;
            case 2:
                if (!stageActive)
                {
                    for (int i = 0; i < deactivateObjects.Length; i++)
                    {
                        deactivateObjects[i].gameObject.SetActive(false);
                    }

                    camCon.LerpToZoom(2f, 5f); //zoom out exterior
                    camCon.LerpToPositionY(2f, 2.25f);
                    fadeOut.EventEnter(playerData.gameObject); //fade out scene and family
                    fadeIn.EventEnter(playerData.gameObject); //fade in hospital exterior
                    playerData.UnfreezePlayer(); //unfreeze the player
                    //playerData.customPlayerVelocity = -100; //make the player very slow
                    stageActive = true;
                }
                //else if (SystemSettings.moveRight || SystemSettings.moveLeft)
                //{
                //    //SpriteLerp();
                //}

                if (!camCon.GetZoomStatus())
                {
                    playerData.customPlayerVelocity = 0;
                    stage = 3;
                    stageActive = false;
                }
                break;
            default:
                break;
        }
    }

    public override void EventStay(GameObject playerGO)
    {
        if (stage == 0)
        {
            stage = 1;
        }
    }
}
