using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverScriptsPlayer : MonoBehaviour
{
    public List<VoiceOverEnum> voiceOverSequence = new List<VoiceOverEnum>(4);
    public LevelLoader levelLoader;

    private int currentIndex = 0;
    private AudioSource voiceOverSource;

    public bool IsSequenceComplete { get; private set; } = false;
    private float totalDuration = 0f;
    private float elapsedTime = 0f;

    void Start()
    {
        voiceOverSource = AudioManager.Instance.VoiceOverAudioSource;

        if (voiceOverSequence.Count > 0)
        {
            CalculateTotalDuration();
            StartCoroutine(PlayVoiceOverSequence());
        }
    }

    public void StartSequence(List<VoiceOverEnum> sequence)
    {
        voiceOverSequence = sequence;
        currentIndex = 0;
        elapsedTime = 0f;
        IsSequenceComplete = false;
        CalculateTotalDuration();
        StartCoroutine(PlayVoiceOverSequence());
    }

    private void CalculateTotalDuration()
    {
        totalDuration = 0f;
        foreach (var clipEnum in voiceOverSequence)
        {
            var clip = AudioManager.GetVoiceOverClip(clipEnum);
            if (clip != null)
                totalDuration += clip.length;
        }
    }

    public float GetProgress()
    {
        return totalDuration > 0f ? Mathf.Clamp01(elapsedTime / totalDuration) : 0f;
    }

    private IEnumerator PlayVoiceOverSequence()
    {
        while (currentIndex < voiceOverSequence.Count)
        {
            AudioManager.PlayVoiceOverWithSubtitles(voiceOverSequence[currentIndex]);
            var clip = AudioManager.GetVoiceOverClip(voiceOverSequence[currentIndex]);

            float timer = 0f;
            float clipLength = clip != null ? clip.length : 0f;

            // Hybrid waiting: ensure we wait for the clip's duration, but also respect isPlaying
            while (voiceOverSource.isPlaying || timer < clipLength)
            {
                timer += Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentIndex++;
        }

        IsSequenceComplete = true;

        // Load the next level once voiceovers are done
        if (levelLoader != null)
        {
            
            Invoke(nameof(LoadNextLevel), 5f);
        }
        else
        {
            Debug.LogWarning("LevelLoader is not assigned to VoiceOverScriptsPlayer.");
        }
    }

    private void LoadNextLevel()
    {
        levelLoader.LoadNextLevel();
    }
}