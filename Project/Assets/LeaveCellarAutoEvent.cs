using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCellarAutoEvent : AutoEvent
{
    PlayerData playerData;

    int stage = 0;
    bool stageActive = false;

    float stage3Count = 5f;
    float stage3Subtractor = 0.5f;
    bool stage3End = false;
    CameraController camCon;

    private void Start()
    {
        playerData = GameManager.playerData;
        camCon = GameManager.mainCamera.GetComponent<CameraController>();
    }

    public override void EventEnter(GameObject playerGO)
    {
        //AudioManager.PlayVoiceOverWithSubtitles(VoiceOverEnum.Level3Track4); //testimony begins
        camCon.LerpToZoom(0.8f, 3f); //zoom out exterior
        camCon.LerpToPosition(0.8f, new(60f, -4f, -10));
        camCon.BeginFollow();
        playerData.UnfreezePlayer(); //unfreeze the player
        playerData.gameObject.transform.position = new Vector3(46.25f, -1f, 0);
    }
}