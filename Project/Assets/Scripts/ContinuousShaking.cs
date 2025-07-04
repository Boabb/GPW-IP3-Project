using UnityEngine;

public class ContinuousShaking : MonoBehaviour
{
    private CameraController cameraController;
    private PlayerData playerData;
    private float timeToNextShake = 3.0f;

    // Start is called before the first frame update
    private void Start()
    {
        cameraController = GameManager.mainCamera.GetComponent<CameraController>();
        playerData = GameManager.playerData;
    }

    // Update is called once per frame
    private void Update()
    {
        timeToNextShake -= Time.deltaTime;
		if (timeToNextShake <= 0.0f)
        {
            timeToNextShake = Random.Range(1.5f, 6.0f);
            cameraController.StartPlayerFocusShake(Random.Range(0.2f, 0.5f), 0.3f, 1);
		}

        cameraController.SetCameraPosition(new Vector3(playerData.playerRigidbody.position.x, 0.89f));
    }
}
