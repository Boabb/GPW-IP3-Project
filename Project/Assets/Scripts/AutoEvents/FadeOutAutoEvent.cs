using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAutoEvent : AutoEvent
{
    [SerializeField] SpriteRenderer[] fadeObject;

    float currentFadeAmount = 0f;
    bool fade = false;
    [SerializeField] float fadeScalar = 1f;

    private void Start()
    {
        for (int i = 0; i < fadeObject.Length; i++)
        {
            fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, 1);
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
                    fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, 0);
                }
            }
            else
            {
                currentFadeAmount += fadeScalar * Time.deltaTime;
            }

            if (fade)
            {
                for (int i = 0; i < fadeObject.Length; i++)
                {
                    fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, 1 - currentFadeAmount);
                }
            }
        }
    }

    public override void EventEnter(GameObject playerGO)
    {
        fade = true;
    }
}
