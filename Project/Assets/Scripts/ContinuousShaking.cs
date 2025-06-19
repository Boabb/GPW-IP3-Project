using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousShaking : MonoBehaviour
{
    CameraController cameraController;
    PlayerData playerData;
    bool shake = false;
    // Start is called before the first frame update
    void Start()
    {
        cameraController = GameManager.mainCamera.GetComponent<CameraController>();
        playerData = GameManager.playerData;
        cameraController.SetYOffset(0.86f);
    }

    // Update is called once per frame
    void Update()
    {
        System.Random rand = new System.Random();

        if (rand.Next(500) == 1 && !cameraController.GetShakeStatus())
        {
            int shakeTimeInt = rand.Next(2, 5);

            Debug.Log(shakeTimeInt);
            cameraController.StartPlayerFocusShake((float)shakeTimeInt / 10, 0.3f, 1);

            //cameraController.StartShake(rand.Next(5, 10) / 10, rand.Next(1) / 10, 0.5f);
        }


        cameraController.SetCameraPosition(new Vector3(playerData.playerRigidbody.position.x, 0.89f), false);

        Debug.Log("Shake: " + cameraController.GetShakeStatus());
    }
}
