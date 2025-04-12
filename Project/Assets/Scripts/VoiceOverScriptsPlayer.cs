using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverScriptsPlayer : MonoBehaviour
{
    public List<VoiceOverEnum> voiceOverSequence = new List<VoiceOverEnum>(4);

    private int currentIndex = 0;
    private AudioSource voiceOverSource;

    void Start()
    {
        // Get the reference from AudioManager
        voiceOverSource = AudioManager.Instance.VoiceOverAudioSource;

        // Optional: Auto-play on start if list is populated
        if (voiceOverSequence.Count > 0)
        {
            StartCoroutine(PlayVoiceOverSequence());
        }
    }

    public void StartSequence(List<VoiceOverEnum> sequence)
    {
        voiceOverSequence = sequence;
        currentIndex = 0;
        StartCoroutine(PlayVoiceOverSequence());
    }

    private IEnumerator PlayVoiceOverSequence()
    {
        while (currentIndex < voiceOverSequence.Count)
        {
            AudioManager.PlayVoiceOverWithSubtitles(voiceOverSequence[currentIndex]);
            yield return new WaitUntil(() => !voiceOverSource.isPlaying);
            currentIndex++;
        }
    }
}
