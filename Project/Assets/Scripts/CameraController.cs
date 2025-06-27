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
    float cameraFollowSpeed = 2.5f;
	Vector3 camPosition;

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
    float m_positionLerpTime;

    float m_positionLerpCounter;
    bool m_positionLerp;
    public void LerpToPosition(float lerpTime, Vector3 position) //lerps from current position to a stationary position
    {
		m_positionToLerpTo = position;
		m_positionToLerpTo.z = mainCamera.transform.position.z;
		m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpTime = lerpTime;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollow();
    }

    public void LerpToPosition(float lerpTime)
    {
        m_positionToLerpTo = player.transform.position;
        m_positionToLerpTo.z = mainCamera.transform.position.z;

		m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpTime = lerpTime;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollow();
    }

    public void LerpToPositionY(float lerpTime, float position)
    {
		m_positionToLerpFrom = mainCamera.transform.position;
		m_positionToLerpTo = new Vector3(m_positionToLerpFrom.x, position, m_positionToLerpFrom.z);
        m_positionLerpTime = lerpTime;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollowY();
    }

    public void LerpToPositionY(float lerpTime)
    {
        m_positionToLerpTo = new Vector3(mainCamera.transform.position.x, standardY, mainCamera.transform.position.z);
        m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpTime = lerpTime;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollowY();
    }

    public void LerpToPositionX(float lerpTime, float position)
    {
        m_positionToLerpTo = new Vector3(position, mainCamera.transform.position.y, mainCamera.transform.position.z);
        m_positionToLerpFrom = mainCamera.transform.position;
        m_positionLerpTime = lerpTime;

        m_positionLerpCounter = 0;
        m_positionLerp = true;
        StopFollowX();
    }

    float m_zoomToLerpTo;
    float m_zoomToLerpFrom;
    float m_zoomLerpTime;

    float m_zoomLerpCounter;
    bool m_zoomLerp;
    public void LerpToZoom(float lerpTime, float zoom) //lerps from current zoom to a given zoom
    {
        m_zoomToLerpTo = zoom;
        m_zoomToLerpFrom = mainCamera.orthographicSize;
        m_zoomLerpTime = lerpTime;

        m_zoomLerpCounter = 0;
        m_zoomLerp = true;
    }

    public void LerpToZoom(float lerpTime)
    {
        m_zoomToLerpTo = standardZoom;
        m_zoomToLerpFrom = mainCamera.orthographicSize;
        m_zoomLerpTime = lerpTime;

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
        camPosition = transform.position;
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

    public void SetCameraPosition(Vector3 camPos)
    {
         gameObject.transform.position = new Vector3(camPos.x, camPos.y, gameObject.transform.position.z);
	}

    //private methods
    void Follow() //follows the player
    {
		if (player.transform.position.x > levelUpperLimit)
		{
			return;
		}

		if (player.transform.position.x < levelLowerLimit)
		{
			return;
		}

		Vector3 position = transform.position;

        if (m_followX)
        {
            position.x = player.transform.position.x;
        }

        if (m_followY)
        {
            position.y = player.transform.position.y + standardY;
        }

        transform.position += Vector3.MoveTowards(transform.position, position, cameraFollowSpeed * Time.deltaTime) - transform.position;
	}

    void Shake()
    {
        if (m_shakeTime > 0)
        {
            transform.position = new Vector3(camPosition.x + Random.insideUnitSphere.x * m_shakeAmount, camPosition.y + Random.insideUnitSphere.y * m_shakeAmount, transform.position.z);
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
            transform.localPosition = new Vector3(player.transform.position.x + Random.insideUnitSphere.x * m_shakeAmount * 2, 0.89f + Random.insideUnitSphere.y * m_shakeAmount * 0.75f, transform.localPosition.z);
            m_shakeTime -= Time.deltaTime * m_shakeDecrease;
		}
        else
        {
            m_shakeTime = 0.0f;
            m_playerShake = false;
        }
    }

    void PositionLerp()
    {
        if (m_positionLerpCounter < m_positionLerpTime)
        {
            mainCamera.transform.position = Vector3.Lerp(m_positionToLerpFrom, m_positionToLerpTo, m_positionLerpCounter / m_positionLerpTime);
            m_positionLerpCounter += Time.deltaTime;
		}
        else
        {
            mainCamera.transform.position = m_positionToLerpTo;
            m_positionLerp = false;
        }
    }

    void ZoomLerp()
    {
        if (m_zoomLerpCounter < m_zoomLerpTime)
        {
            mainCamera.orthographicSize = Mathf.Lerp(m_zoomToLerpFrom, m_zoomToLerpTo, m_zoomLerpCounter / m_zoomLerpTime);
            m_zoomLerpCounter += Time.deltaTime;
        }
        else
        {
            mainCamera.orthographicSize = m_zoomToLerpTo;
            m_zoomLerp = false;
        }
    }
}
