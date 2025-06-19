using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3EndAutoEvent : AutoEvent
{
    int stage = 0;
    bool stageActive = false;
    bool end = false;

    PlayerData playerData;
    [SerializeField] AudioSource testimonyAudioSource;
    [SerializeField] LevelLoader levelLoader;

    private void Start()
    {
        playerData = GameManager.playerData;
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
                    StartCoroutine(TimeEnd());
                    stageActive = true;
                }
                else if (end && !testimonyAudioSource.isPlaying)
                {
                    stage = 2;
                    stageActive = false;
                }
                break;
            case 2:
                if (!stageActive)
                {
                    levelLoader.LoadNextLevel();
                    stageActive = true;
                }
                break;
        }
    }

    public override void EventEnter(GameObject playerGO)
    {
        stage = 1;
    }

    IEnumerator TimeEnd()
    {
        yield return new WaitForSeconds(5f);
        end = true;
    }
}
