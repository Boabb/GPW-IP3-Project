using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FadeInAutoEvent : AutoEvent
{
    public static FadeInAutoEvent instance;
    [SerializeField] SpriteRenderer[] fadeObject;

    float currentFadeAmount = 0f;
    bool fade = false;
    [SerializeField] float fadeScalar = 1f;

    private void Start()
    {
        instance = this;
        for (int i = 0; i < fadeObject.Length; i++)
        {
            fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, 0);
        }
    }

    private void Update()
    {
        if (fade)
        {
            Debug.Log("CurrentFade: " + currentFadeAmount);
            if (currentFadeAmount >= 1)
            {
                currentFadeAmount = 0;
                fade = false;

                for (int i = 0; i < fadeObject.Length; i++)
                {
                    fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, 1);
                }
            }
            else
            {
                currentFadeAmount += fadeScalar * Time.deltaTime;
            }

            if(fade)
            {
                for (int i = 0; i < fadeObject.Length; i++)
                {
                    fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, currentFadeAmount);
                }
            }
        }
    }

    public static void TriggerFade()
    {
        instance.fade = true;
    }
}
