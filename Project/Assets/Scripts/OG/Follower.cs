using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] GameObject agentToFollow;
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 0;
    [SerializeField] float cameraSpeed = 10;

    bool shake = false;
    float shakeTime;
    float shakeAmount;
    float decreaseShake;
    // Start is called before the first frame update
    void Start()
    {
        if (agentToFollow != null)
        {
            agentToFollow = GameManager.player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agentToFollow != null)
        {
            Vector3 position = new(agentToFollow.transform.position.x, agentToFollow.transform.position.y, transform.position.z);
            position.x -= xOffset;
            position.y -= yOffset;

            transform.position = Vector3.Lerp(transform.position, position, cameraSpeed/10);
        }
        else
        {
            agentToFollow = GameManager.player;
        }

        if (shake)
        {
            Shake();
        }
    }

    //https://discussions.unity.com/t/screen-shake-effect/391783/5
    public void StartShake()
    {
        shakeTime = 2;
        shakeAmount = 0.3f;
        decreaseShake = 1;

        shake = true;
    }

    void Shake()
    {
        if (shakeTime > 0)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + Random.insideUnitSphere.x * shakeAmount, gameObject.transform.localPosition.y + Random.insideUnitSphere.y * shakeAmount, gameObject.transform.localPosition.z);
            shakeTime -= Time.deltaTime * decreaseShake;
        }
        else
        {
            shakeTime = 0.0f;
            shake = false;
        }
    }
}
