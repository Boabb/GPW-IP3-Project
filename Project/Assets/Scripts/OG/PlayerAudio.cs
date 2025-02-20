using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource woodenScrapeClip;
    [SerializeField] AudioSource woodenJumpClip;
    [SerializeField] AudioSource woodenFootstepsClip;
    [SerializeField] AudioSource crawlInVentClip;
    
    static bool woodenScrape;
    static bool woodenJump;
    static bool woodenFootsteps;
    static bool crawlInVent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (woodenScrape)
        {
            if (!woodenScrapeClip.isPlaying)
            {
                woodenScrapeClip.Play();
            }
;       }
        else
        {
            woodenScrapeClip.Pause();
        }

        if (woodenJump)
        {
            woodenJumpClip.Play();
            woodenJump = false;
        }

        if (woodenFootsteps)
        {
            if (!woodenFootstepsClip.isPlaying)
            {
                woodenFootstepsClip.Play();
            }
        }
        else
        {
            woodenFootstepsClip.Pause();
        }

        if (crawlInVent)
        {
            if (!crawlInVentClip.isPlaying)
            {
                crawlInVentClip.Play();
            }
        }
        else
        {
            crawlInVentClip.Pause();
        }
    }

    public static void PlayWoodenScrape(bool check)
    {
        woodenScrape = check;
    }

    public static void PlayWoodenJump()
    {
        woodenJump = true;
    }

    public static void PlayWoodenFootsteps(bool check)
    {
        woodenFootsteps = check;
    }

    public static void PlayCrawlInVent(bool check)
    {
        crawlInVent = check;
    }
}
