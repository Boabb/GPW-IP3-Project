using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAndOut : AutoEvent
{
    public static FadeInAndOut instance;
    [SerializeField] SpriteRenderer[] fadeObject;
    [SerializeField] GameObject[] objectsToActivate;
    [SerializeField] GameObject[] objectsToDeactivate;

    float currentFadeAmount = 0f;
    bool fadeIn = false;
    bool fadeOut = false;
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
        if (fadeIn)
        {
            //StartCoroutine(GameManager.TransitionToOutsideSection(12f));
            //Debug.Log("CurrentFade: " + currentFadeAmount);
            if (currentFadeAmount >= 1)
            {
                currentFadeAmount = 0;
                fadeIn = false;
                fadeOut = true;

                for (int i = 0; i < fadeObject.Length; i++)
                {
                    fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, 1);
                }

                for(int i = 0;i < objectsToActivate.Length;i++)
                {
                    objectsToActivate[i].SetActive(true);
                }

                for (int i = 0; i< objectsToDeactivate.Length;i++)
                {
                    objectsToDeactivate[i].SetActive(false);
                }
            }
            else
            {
                currentFadeAmount += fadeScalar * Time.deltaTime;
            }

            if (fadeIn)
            {
                for (int i = 0; i < fadeObject.Length; i++)
                {
                    fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, currentFadeAmount);
                }
            }
        }

        if (fadeOut)
        {
            //Debug.Log("CurrentFade: " + currentFadeAmount);
            if (currentFadeAmount >= 1)
            {
                currentFadeAmount = 0;
                fadeOut = false;

                for (int i = 0; i < fadeObject.Length; i++)
                {
                    fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, 0);
                }
            }
            else
            {
                currentFadeAmount += fadeScalar * Time.deltaTime;
            }

            if (fadeOut)
            {
                for (int i = 0; i < fadeObject.Length; i++)
                {
                    fadeObject[i].color = new Color(fadeObject[i].color.r, fadeObject[i].color.g, fadeObject[i].color.b, 1 - currentFadeAmount);
                }
            }
        }
    }

    public static void TriggerFade()
    {
        if (instance != null)
        {
            instance.fadeIn = true;
        }
        else
        {

        }
    }

    public override void EventEnter(GameObject playerGO)
    {
        fadeIn = true;
    }
}
