using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartVoiceOverPlayer : MonoBehaviour
{
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private int trackNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        PlayLevelVoiceOver();
    }

    private void PlayLevelVoiceOver()
    {
        string clipName = $"Level{levelNumber}Track{trackNumber}";

        if (System.Enum.TryParse(clipName, out VoiceOverEnum voiceOverEnum))
        {
            AudioManager.PlayVoiceOverWithSubtitles(voiceOverEnum, 1f);
            Debug.Log($"Now playing {clipName}");
        }
        else
        {
            Debug.LogWarning($"No matching voice-over found for: {clipName}");
        }
    }
}
