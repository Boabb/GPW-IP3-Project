using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScrollUI : MonoBehaviour
{
    public ScrollRect scrollRect;
    public VoiceOverScriptsPlayer voiceOverPlayer;

    void Update()
    {
        if (scrollRect != null && voiceOverPlayer != null)
        {
            float progress = voiceOverPlayer.GetProgress();
            scrollRect.horizontalNormalizedPosition = progress;
        }
    }
}