using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Camera mainCamera;
    float lerpCounter;
    float standardZoom = 3.1f;
    float standardY = 0.86f;
    float xOffset = 0;
    float yOffset = 0.86f;
    float cameraSpeed = 10;

    [SerializeField] float levelUpperLimit = 114.6f;
    [SerializeField] float levelLowerLimit = 2.13f;

    private void Start()
    {
        player = PlayerData.PlayerGO;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (m_positionLerp)
        {
            PositionLerp();
        }

        if (m_zoomLerp)
        {
            ZoomLerp();
        }

        if (m_followX || m_followY)
        {
            Follow();
        }

        if (m_shake)
        {
            Shake();
        }

        if (m_playerShake)
        {
            PlayerFocusShake();
        }
    }

    //camera controls
    public void Stationary(Vector3 position) //sets the camera at a specific location
    {
        mainCamera.transform.position = position;
        StopFollow();
    }

    public void Stationary(float zoom) //sets the camera at a specific zoom
    {
        mainCamera.orthographicSize = zoom;
        StopFollow();
    }

    public void Stationary(Vector3 position, float zoom) //sets the camera at a specific location and zoom
    {
        mainCamera.orthographicSize = zoom;
        mainCamera.transform.position = position;
        StopFollow();
    }

    Vector3 m_positionToLerpTo;
    Vector3 m_positionToLerpFrom;
    float m_positionLerpSpeed;

    float m_positionLerpCounter;
    bool m_positionLerp;
    public void LerpToPosition(float lerpSpeed, Vector3 position) //lerps from current position to a stationary position
    {
        m_positionToLerpTo = position;
        m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpSpeed = lerpSpeed;

        yOffset = position.y;
        xOffset = position.x;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollow();
    }

    public void LerpToPosition(float lerpSpeed)
    {
        m_positionToLerpTo = player.transform.position;
        m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpSpeed = lerpSpeed;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollow();
    }

    public void LerpToPositionY(float lerpSpeed, float position)
    {
        m_positionToLerpTo = new Vector3(mainCamera.transform.position.x, position, mainCamera.transform.position.z);
        m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpSpeed = lerpSpeed;

        yOffset = position;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollowY();
    }

    public void LerpToPositionY(float lerpSpeed)
    {
        m_positionToLerpTo = new Vector3(mainCamera.transform.position.x, standardY, mainCamera.transform.position.z);
        m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpSpeed = lerpSpeed;

        yOffset = 0;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollowY();
    }

    public void LerpToPositionX(float lerpSpeed, float position)
    {
        m_positionToLerpTo = new Vector3(position, mainCamera.transform.position.y);
        m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpSpeed = lerpSpeed;

        xOffset = position;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollowX();
    }

    float m_zoomToLerpTo;
    float m_zoomToLerpFrom;
    float m_zoomLerpSpeed;

    float m_zoomLerpCounter;
    bool m_zoomLerp;
    public void LerpToZoom(float lerpSpeed, float zoom) //lerps from current zoom to a given zoom
    {
        m_zoomToLerpTo = zoom;
        m_zoomToLerpFrom = mainCamera.orthographicSize;
        m_zoomLerpSpeed = lerpSpeed;

        m_zoomLerpCounter = 0;
        m_zoomLerp = true;
    }

    public void LerpToZoom(float lerpSpeed)
    {
        m_zoomToLerpTo = standardZoom;
        m_zoomToLerpFrom = mainCamera.orthographicSize;
        m_zoomLerpSpeed = lerpSpeed;

        m_zoomLerpCounter = 0;
        m_zoomLerp = true;
    }

    public bool GetZoomStatus()
    {
        return m_zoomLerp;
    }

    public bool GetShakeStatus()
    {
        if (m_shake || m_playerShake)
        {
            return true;
        }
        return false;
    }    

    //public bool m_follow = true;
    bool m_followY = false;
    bool m_followX = true;

    public void BeginFollow()
    {
        BeginFollowX();
        BeginFollowY();
    }    

    public void BeginFollowX()
    {
        m_followX = true;
    }

    public void BeginFollowY()
    {
        m_followY = true;
    }

    public void StopFollow()
    {
        StopFollowX();
        StopFollowY();
    }
    public void StopFollowX()
    {
        m_followX = false;
    }    

    public void StopFollowY()
    {
        m_followY = false;
    }

    //set offsets
    public void SetYOffset(float setOffset)
    {
        yOffset = setOffset;
    }

    public void SetXOffset(float setOffset)
    {
        xOffset = setOffset;
    }

    //camera effects
    //https://discussions.unity.com/t/screen-shake-effect/391783/5
    float m_shakeTime;
    float m_shakeAmount;
    float m_shakeDecrease;
    bool m_shake;
    public void StartShake(float shakeTime, float shakeAmount, float shakeDecrease)
    {
        m_shakeTime = shakeTime;
        m_shakeAmount = shakeAmount;
        m_shakeDecrease = shakeDecrease;
        camPosition = gameObject.transform.position;
        m_shake = true;
    }

    bool m_playerShake;
    public void StartPlayerFocusShake(float shakeTime, float shakeAmount, float shakeDecrease)
    {
        m_shakeTime = shakeTime;
        m_shakeAmount = shakeAmount;
        m_shakeDecrease = shakeDecrease;
        m_playerShake = true;
    }

    public void SetCameraPosition(Vector3 camPos, bool useOffsets)
    {
        if (useOffsets)
        {
            gameObject.transform.position = new Vector3(camPos.x + xOffset, camPos.y + yOffset, gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(camPos.x, camPos.y, gameObject.transform.position.z);
        }
    }

    //private methods
    void Follow() //follows the player
    {
        Vector3 position = transform.position;

        if (m_followX)
        {
            position = new Vector3(player.transform.position.x, position.y, position.z);
        }

        if (m_followY)
        {
            position = new Vector3(position.x, player.transform.position.y + standardY, position.z);
        }

        if (player.transform.position.x > levelUpperLimit)
        {
            return;
        }

        if (player.transform.position.x < levelLowerLimit)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, position, cameraSpeed / 10);
    }

    Vector3 camPosition;
    void Shake()
    {
        if (m_shakeTime > 0)
        {
            gameObject.transform.localPosition = new Vector3(camPosition.x + Random.insideUnitSphere.x * m_shakeAmount, camPosition.y + Random.insideUnitSphere.y * m_shakeAmount, gameObject.transform.localPosition.z);
            m_shakeTime -= Time.deltaTime * m_shakeDecrease;
        }
        else
        {
            m_shakeTime = 0.0f;
            m_shake = false;
        }
    }

    void PlayerFocusShake()
    {
        if (m_shakeTime > 0)
        {
            gameObject.transform.localPosition = new Vector3(player.transform.position.x + Random.insideUnitSphere.x * m_shakeAmount * 2, player.transform.position.y + standardY + yOffset + Random.insideUnitSphere.y * m_shakeAmount * 0.5f, gameObject.transform.localPosition.z);
            m_shakeTime -= Time.deltaTime * m_shakeDecrease;
        }
        else
        {
            m_shakeTime = 0.0f;
            m_playerShake = false;
            //gameObject.transform.localPosition = new Vector3(player.transform.position.x, yOffset, gameObject.transform.localPosition.z);
        }
    }

    void PositionLerp()
    {
        if (m_positionLerpCounter < 1)
        {
            mainCamera.transform.position = Vector3.Lerp(m_positionToLerpFrom, m_positionToLerpTo, m_positionLerpCounter);
            m_positionLerpCounter += m_positionLerpSpeed * Time.deltaTime;
        }
        else
        {
            mainCamera.transform.position = m_positionToLerpTo;
            m_positionLerp = false;
        }
    }

    void ZoomLerp()
    {
        if (m_zoomLerpCounter < 1)
        {
            mainCamera.orthographicSize = Mathf.Lerp(m_zoomToLerpFrom, m_zoomToLerpTo, m_zoomLerpCounter);
            m_zoomLerpCounter += m_zoomLerpSpeed * Time.deltaTime;
        }
        else
        {
            mainCamera.orthographicSize = m_zoomToLerpTo;
            m_zoomLerp = false;
        }
    }
}
