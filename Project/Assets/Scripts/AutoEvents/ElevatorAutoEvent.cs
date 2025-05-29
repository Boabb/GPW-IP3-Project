using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorAutoEvent : AutoEvent
{
    [SerializeField] Collider2D armChair;
    Collider2D elevatorCollider;
    PlayerData playerData;
    public Animator openCloseAnimator;

    int stage = 0;
    bool stageActive = false;

    float stage3Count = 5f;
    float stage3Subtractor = 0.5f;
    bool stage3End = false;
    [SerializeField] SpriteRenderer[] elevatorRenderers;
    //[SerializeField] SpriteRenderer[] fadeRenderers;
    [SerializeField] FadeInAutoEvent fadeIn;
    [SerializeField] FadeOutAutoEvent fadeOut;
    [SerializeField] AudioSource testimonyAudioSource;
    [SerializeField] Collider2D[] collidersToDeactivate;
    CameraController camCon;

    private void Start()
    {
        playerData = GameManager.playerData;
        camCon = GameManager.mainCamera.GetComponent<CameraController>();
        elevatorCollider = gameObject.GetComponent<Collider2D>();
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
                    playerData.FreezePlayer();

                    //doors open
                    openCloseAnimator.SetTrigger("Open/Close");
                    stageActive = true;
                }

                if (openCloseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Elevator Open"))
                {
                    openCloseAnimator.ResetTrigger("Open/Close");
                    stage = 2;
                    stageActive = false;
                }
                break;
            case 2:
                //zoom, doors close,
                if (!stageActive)
                {
                    //puts the doors and the elevator front into the foreground
                    for (int i = 0; i < elevatorRenderers.Length; i++)
                    {
                        elevatorRenderers[i].sortingLayerID = SortingLayer.NameToID("Foreground");
                    }

                    openCloseAnimator.SetTrigger("Open/Close");
                    camCon.LerpToZoom(0.5f, 1.6f);
                    camCon.LerpToPositionY(0.5f, 0.35f);
                    stageActive = true;
                }
                
                if (openCloseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Elevator Closed"))
                {
                    openCloseAnimator.ResetTrigger("Open/Close");
                    stage = 3;
                    stageActive = false;
                }
                break;
            case 3:
                if (!stageActive)
                {
                    fadeIn.EventEnter(playerData.gameObject);
                    stageActive = true;
                    StartCoroutine(GameManager.TransitionToOutsideSection(4));
                }
                //fade, shake and lift details move, continue until testimony is complete
                camCon.SetYOffset(0.86f);
                camCon.SetCameraPosition(playerData.transform.position, true);
                camCon.StartShake(1, 0.05f, 1);
                stage3Count -= stage3Subtractor * Time.deltaTime;

                if (stage3Count < 0)
                {
                    stage3End = true;
                }

                if (!testimonyAudioSource.isPlaying && stage3End)
                {
                    stage = 4;
                    stageActive = false;
                }
                break;
            case 4:
                //ding effect, doors open, fade, undo zoom
                if (!stageActive)
                {
                    openCloseAnimator.SetTrigger("Open/Close");
                    fadeOut.EventEnter(playerData.gameObject);
                    camCon.LerpToPositionY(0.5f);
                    stageActive = true;
                }

                if (openCloseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Elevator Open"))
                {
                    openCloseAnimator.ResetTrigger("Open/Close");
                    stage = 5;
                    stageActive = false;
                }
                break;
            case 5:
                //doors and lift go back to background, player unfreezes
                if (!stageActive)
                {
                    playerData.UnfreezePlayer();
                    camCon.LerpToZoom(0.5f);
                    openCloseAnimator.SetTrigger("Open/Close");

                    for (int i = 0; i < elevatorRenderers.Length; i++)
                    {
                        elevatorRenderers[i].sortingLayerID = SortingLayer.NameToID("Background");
                    }

                    for (int i = 0; i < collidersToDeactivate.Length; i++)
                    {
                        collidersToDeactivate[i].enabled = false;
                    }

                    stageActive = true;
                }

                if (openCloseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Elevator Closed"))
                {
                    openCloseAnimator.ResetTrigger("Open/Close");
                    stage = 6;
                    stageActive = false;
                }
                //doors close
                break;
            default:
                break;
        }
    }

    public override void EventStay(GameObject playerGO)
    {
        if (elevatorCollider.bounds.Intersects(armChair.bounds) && stage == 0)
        {
            stage = 1;
        }
    }
}
