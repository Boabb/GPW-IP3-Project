using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramAutoEvent : AutoEvent
{
    PlayerData playerData;

    int stage = 0;
    bool stageActive = false;

    bool stage3End = false;

    [SerializeField] FadeOutAutoEvent fadeOut;
    [SerializeField] FadeInAutoEvent fadeIn;
    [SerializeField] GameObject[] familyRenderers;
    Vector3[] ogFamilyPositions;
    [SerializeField] GameObject[] toDeactivate;
    CameraController camCon;

    private void Start()
    {
        playerData = GameManager.playerData;
        camCon = GameManager.mainCamera.GetComponent<CameraController>();

        ogFamilyPositions = new Vector3[familyRenderers.Length];

        for (int i = 0; i < familyRenderers.Length; i++)
        {
            ogFamilyPositions[i] = familyRenderers[i].transform.localPosition;
        }
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
                    fadeIn.EventEnter(playerData.gameObject);
                    //camCon.LerpToPositionY(0.2f, 2.25f);
                    fadeOut.EventEnter(playerData.gameObject); //fade out scene and family
                    //playerData.customPlayerVelocity = -100; //make the player very slow
                    stageActive = true;

                    for (int i = 0; i < toDeactivate.Length; i ++)
                    {
                        toDeactivate[i].SetActive(false);
                    }
                }
                else
                {
                    SpriteLerp();
                }

                if (!spriteLerpToggle)
                {
					playerData.UnfreezePlayer();
					playerData.customPlayerVelocity = -40;
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
                familyRenderers[i].transform.localPosition = Vector3.Lerp(ogFamilyPositions[i], new Vector3(ogFamilyPositions[i].x, -1.36f, 1), lerpCounter);
            }
            lerpCounter += 0.3f * Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < familyRenderers.Length; i++)
            {
                familyRenderers[i].transform.localPosition = new Vector3(ogFamilyPositions[i].x, -1.36f, 1);
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
