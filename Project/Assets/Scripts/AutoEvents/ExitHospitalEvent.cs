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
                    AudioManager.PlayVoiceOverAudio(VoiceOverEnum.Level2Track4); //testimony begins
                    camCon.LerpToZoom(0.2f, 2); //zoom on family and door
                    fadeOut.EventEnter(playerData.gameObject); //fade out scene and family
                    fadeIn.EventEnter(playerData.gameObject); //fade in hospital exterior

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
                    //camCon.LerpToZoom(0.5f, 10f); //zoom out exterior
                    playerData.UnfreezePlayer(); //unfreeze the player
                    camCon.LerpToZoom(0.2f, 3); //zoom on family and door
                    playerData.customPlayerVelocity = -100; //make the player very slow
                    stageActive = true;
                }
                else if (SystemSettings.moveRight || SystemSettings.moveLeft)
                {
                    SpriteLerp();
                }

                if (!spriteLerpToggle)
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

    float lerpCounter = 0;
    bool spriteLerpToggle = true;
    void SpriteLerp()
    {
        if (lerpCounter < 1)
        {
            for (int i = 0; i < familyRenderers.Length; i++)
            {
                familyRenderers[i].transform.localScale = Vector3.Lerp(new Vector3(0.05f, 0.05f, 1), new Vector3(0.19f, 0.19f, 1), lerpCounter);
            }
            lerpCounter += 0.05f * Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < familyRenderers.Length; i++)
            {
                familyRenderers[i].transform.localScale = new Vector3(0.19f, 0.19f, 1);
            }

            spriteLerpToggle = false;
        }
    }
}
