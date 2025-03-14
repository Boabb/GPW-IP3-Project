using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Camera mainCamera;
    float lerpCounter;
    float standardZoom = 3f;
    float xOffset = 0;
    float yOffset = 0;
    float cameraSpeed = 10;

    bool freezeYFollow = true; //keeps the y coord stationary even when following
    float levelUpperLimit = 48.03f;
    float levelLowerLimit = 6.21f;

    private void Start()
    {
        player = PlayerData.PlayerGO;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (m_shake)
        {
            Shake();
        }

        if (m_follow)
        {
            Follow();
        }

        if (m_positionLerp)
        {
            PositionLerp();
        }

        if (m_zoomLerp)
        {
            ZoomLerp();
        }
    }

    //camera controls
    public void Stationary(Vector3 position) //sets the camera at a specific location
    {
        mainCamera.transform.position = position;
        m_follow = false;
    }

    public void Stationary(float zoom) //sets the camera at a specific zoom
    {
        mainCamera.orthographicSize = zoom;
        m_follow = false;
    }

    public void Stationary(Vector3 position, float zoom) //sets the camera at a specific location and zoom
    {
        mainCamera.orthographicSize = zoom;
        mainCamera.transform.position = position;
        m_follow = false;
    }

    Vector3 m_positionToLerpTo;
    Vector3 m_positionToLerpFrom;
    float m_positionLerpSpeed;

    float m_positionLerpCounter;
    bool m_positionLerp;
    public void LerpToPosition(Vector3 position, float lerpSpeed) //lerps from current position to a stationary position
    {
        m_positionToLerpTo = position;
        m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpSpeed = lerpSpeed;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        m_follow = false;
    }

    float m_zoomToLerpTo;
    float m_zoomToLerpFrom;
    float m_zoomLerpSpeed;

    float m_zoomLerpCounter;
    bool m_zoomLerp;
    public void LerpToZoom(float zoom, float lerpSpeed) //lerps from current zoom to a given zoom
    {
        m_zoomToLerpTo = zoom;
        m_zoomToLerpFrom = mainCamera.orthographicSize;
        m_zoomLerpSpeed = lerpSpeed;

        m_zoomLerpCounter = 0;
        m_zoomLerp = true;
        m_follow = false;
    }

    bool m_follow = true;
    public void BeginFollow()
    {
        m_follow = true;
        m_positionLerp = false;
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
    float m_shakeTime;
    float m_shakeAmount;
    float m_shakeDecrease;
    bool m_shake;
    public void StartShake(float shakeTime, float shakeAmount, float shakeDecrease)
    {
        m_shakeTime = shakeTime;
        m_shakeAmount = shakeAmount;
        m_shakeDecrease = shakeDecrease;

        m_shake = true;
    }

    //private methods
    void Follow() //follows the player
    {
        Vector3 position;
        if (freezeYFollow)
        {
            position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }

        if (player.transform.position.x > levelUpperLimit)
        {
            return;
        }

        if (player.transform.position.x < levelLowerLimit)
        {
            return;
        }

        position.x -= xOffset;
        position.y -= yOffset;

        transform.position = Vector3.Lerp(transform.position, position, cameraSpeed / 10);
    }

    void Shake()
    {
        if (m_shakeTime > 0)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + Random.insideUnitSphere.x * m_shakeAmount, gameObject.transform.localPosition.y + Random.insideUnitSphere.y * m_shakeAmount, gameObject.transform.localPosition.z);
            m_shakeTime -= Time.deltaTime * m_shakeDecrease;
        }
        else
        {
            m_shakeTime = 0.0f;
            m_shake = false;
        }
    }

    void PositionLerp()
    {
        if (m_positionLerpCounter < 1)
        {
            mainCamera.transform.position = Vector3.Lerp(m_positionToLerpFrom, m_positionToLerpTo, m_positionLerpCounter);
            m_positionLerpCounter += m_positionLerpCounter + (m_positionLerpSpeed * Time.deltaTime);
        }
        else
        {
            m_positionLerp = false;
        }
    }

    void ZoomLerp()
    {
        if (m_zoomLerpCounter < 1)
        {
            mainCamera.orthographicSize = Mathf.Lerp(m_zoomToLerpFrom, m_zoomToLerpTo, m_zoomLerpCounter);
            m_zoomLerpCounter += m_zoomLerpCounter + (m_zoomLerpSpeed * Time.deltaTime);
        }
        else
        {
            m_zoomLerp = false;
        }
    }
}
