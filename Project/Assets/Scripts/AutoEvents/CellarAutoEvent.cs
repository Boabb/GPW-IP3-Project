using UnityEngine;

public class CellarAutoEvent : AutoEvent
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

    //private void Update()
    //{
    //    switch (stage)
    //    {
    //        case 0:
    //            break;
    //        case 1:
    //            if (!stageActive)
    //            {
    //                AudioManager.PlayVoiceOverAudio(VoiceOverEnum.Level3Track4); //testimony begins

    //                stageActive = true;
    //            }

    //            if (!camCon.GetZoomStatus())
    //            {
    //                stage = 2;
    //                stageActive = false;
    //            }
    //            break;
    //        case 2:
    //            if (!stageActive)
    //            {

    //                camCon.LerpToZoom(0.8f, 3f); //zoom out exterior
    //                camCon.LerpToPosition(0.8f, new(60f, -4f, -10));
    //                camCon.BeginFollow();
    //                playerData.UnfreezePlayer(); //unfreeze the player
    //                playerData.customPlayerVelocity = -75; //make the player very slow
    //                stageActive = true;
    //            }
    //            //else if (SystemSettings.moveRight || SystemSettings.moveLeft)
    //            //{
    //            //    //SpriteLerp();
    //            //}

    //            if (!camCon.GetZoomStatus())
    //            {
    //                playerData.customPlayerVelocity = 0;
    //                stage = 3;
    //                stageActive = false;
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public override void EventEnter(GameObject playerGO)
    {
        //AudioManager.PlayVoiceOverWithSubtitles(VoiceOverEnum.Level3Track4); //testimony begins
        camCon.LerpToZoom(0.8f, 3f); //zoom out exterior
        camCon.LerpToPosition(0.8f, new(60f, -4f, -10));
        camCon.BeginFollow();
        playerData.UnfreezePlayer(); //unfreeze the player
        playerData.gameObject.transform.position = new Vector3(49.25f, -3.57f, 0);
        //playerData.customPlayerVelocity = -10; //make the player very slow
    }
}