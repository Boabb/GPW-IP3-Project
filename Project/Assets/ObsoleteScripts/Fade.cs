using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SignedBinary
{
    private double _value;
    public bool reachedLimit = false;

    public double Value
    {
        get => _value;
        set
        {
            if (!reachedLimit)
            {
                if (value <= -1)
                {
                    _value = -1;
                    reachedLimit = true;
                }
                else if (value >= 1)
                {
                    _value = 1;
                    reachedLimit = true;
                }
                else
                {
                    _value = value;
                }
            }
        }
    }
}

public class Fade : MonoBehaviour
{
    [SerializeField] Renderer Fade1;
    [SerializeField] Renderer Fade2;

    Color hidden = new Color(1,1,1,1);
    Color revealed = new Color(1,1,1,0);

    SignedBinary currentFadeAmount = new();
    float fadeScalar = 1f;

    Follower follower;

    public bool collision = false;
    bool switchOverTime = false;
    bool switchBetween = true;

    bool shake = true;
    float shakeScalar = 0;

    private void Awake()
    {

        Fade1.material.color = hidden;
        Fade2.material.color = revealed;
    }

    private void Update()
    {
        if (collision)
        {
            collision = false;
            switchBetween = !switchBetween;
            switchOverTime = true;
            follower = Camera.main.GetOrAddComponent<Follower>();
        }

        if (switchOverTime)
        {
            if (currentFadeAmount.reachedLimit)
            { currentFadeAmount.Value = 1; }
            else
            {
                currentFadeAmount.Value += fadeScalar * Time.deltaTime;
            }

            if (switchBetween)
            {
                Fade1.material.color = new Color(1, 1, 1, (float)currentFadeAmount.Value);
                Fade2.material.color = new Color(1, 1, 1, (float)(1 - currentFadeAmount.Value));
            }
            else
            {
                Fade1.material.color = new Color(1, 1, 1, (float)(1 - currentFadeAmount.Value));
                Fade2.material.color = new Color(1, 1, 1, (float)currentFadeAmount.Value);
            }

            shakeScalar += 1.15f * Time.deltaTime;

            if (shakeScalar > 8 && shake)
            {
                shake = false;
                follower.StartShake();
            }

            if (currentFadeAmount.Value >= 1 && !shake)
            {
                currentFadeAmount.Value = 0;
                switchOverTime = false;
            }
        }


    }

    void QuickSwitch()
    {
        if (switchBetween)
        {
            Fade1.material.color = hidden;
            Fade2.material.color = revealed;
        }
        else
        {
            Fade2.material.color = hidden;
            Fade1.material.color = revealed;
        }
    }


}
