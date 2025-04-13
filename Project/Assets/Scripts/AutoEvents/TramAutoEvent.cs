using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramAutoEvent : AutoEvent
{
    PlayerData playerData;

    int stage = 0;
    bool stageActive = false;

    float stage3Count = 5f;
    float stage3Subtractor = 0.5f;
    bool stage3End = false;

    [SerializeField] FadeOutAutoEvent fadeOut;
    [SerializeField] GameObject[] familyRenderers;
    CameraController camCon;

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
                    camCon.LerpToZoom(0.2f, 3); //zoom on family and door

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
                    camCon.LerpToZoom(0.2f, 3f);
                    SpriteLerp();
                    //camCon.LerpToPositionY(0.2f, 2.25f);
                    fadeOut.EventEnter(playerData.gameObject); //fade out scene and family
                    //playerData.customPlayerVelocity = -100; //make the player very slow
                    stageActive = true;
                }
                else
                {
                    SpriteLerp();
                }

                if (!spriteLerpToggle)
                {
                    playerData.UnfreezePlayer();
                    playerData.customPlayerVelocity = 0;
                    stage = 3;
                    stageActive = false;
                }
                break;
            default:
                break;
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
                familyRenderers[i].transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 1), new Vector3(0, 2.8f, 1), lerpCounter);
            }
            lerpCounter += 0.2f * Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < familyRenderers.Length; i++)
            {
                familyRenderers[i].transform.localPosition = new Vector3(0, 2.8f, 1);
            }

            spriteLerpToggle = false;
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
