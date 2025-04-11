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
    [SerializeField] ActivateObject activateObjectEvent;
    [SerializeField] DeactivateObject deactivateObjectEvent;
    CameraController camCon;

    [SerializeField] GameObject[] familyRenderers;
    [SerializeField] GameObject[] hospitalRenderers;

    [SerializeField] private Animator familyAnimator;

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
                    camCon.LerpToZoom(0.2f, 4); //zoom on family and door
                    camCon.LerpToPositionY(.2f, .8f);
                    activateObjectEvent.Interaction(playerData.gameObject); //fade out scene and family
                    deactivateObjectEvent.Interaction(playerData.gameObject); //fade out scene and family
                    fadeOut.EventEnter(playerData.gameObject); //fade out scene and family
                    fadeIn.EventEnter(playerData.gameObject); //fade in hospital exterior

                    for (int i = 0; i < familyRenderers.Length; i++)
                    {
                        familyRenderers[i].transform.localScale = new Vector3(.8f,.8f, 1);
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
                    //camCon.LerpToZoom(0.5f, 10f); //zoom out exterior
                    playerData.UnfreezePlayer(); //unfreeze the player
                    camCon.LerpToZoom(0.2f, 8); //zoom on family and door
                    camCon.LerpToPositionY(.2f, 5);
                    playerData.customPlayerVelocity = -100; //make the player very slow
                    stageActive = true;
                }
                if (!spriteLerpToggle)
                {
                    playerData.customPlayerVelocity = 0;
                    stage = 3;
                    stageActive = false;
                }
                else
                {
                    SpriteLerp();
                }
                break;
                case 3:
                stageActive = true;
                if(Vector2.Distance(playerData.transform.position, familyRenderers[0].transform.position) <= 1f)
                {
                    familyAnimator.SetBool("Walk", true);
                    camCon.LerpToZoom(0.2f, 4); //zoom on family
                    camCon.transform.position = new(camCon.transform.position.x, 2f, camCon.transform.position.z);
                    camCon.LerpToPositionX(.05f, 16f);
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
            lerpCounter += 0.1f * Time.deltaTime;
        }
        else
        {

            spriteLerpToggle = false;
        }
    }
}
