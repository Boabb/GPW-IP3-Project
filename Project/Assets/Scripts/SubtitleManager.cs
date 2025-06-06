using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;

    [System.Serializable]
    public struct SubtitleData
    {
        public string text;
        public float duration;
    }

    [System.Serializable]
    public struct SubtitleSequence
    {
        public string sequenceName;
        public SubtitleData[] subtitles;
    }

    [Header("Subtitle Settings")]
    public SubtitleSequence[] subtitleSequences;
    public TextMeshProUGUI subtitleText;

    private Dictionary<string, int> sequenceIndexTracker = new Dictionary<string, int>();
    private HashSet<string> activeSequences = new HashSet<string>();
    private Coroutine subtitleCoroutine;

    [SerializeField] private bool playOnStart = true;

    private bool subtitlesEnabled = true; // Global subtitle enabled flag

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Load preference for subtitles
        subtitlesEnabled = PlayerPrefs.GetInt("SubtitlesEnabled", SettingsMenu.initialSubtitleActiveStateAsInt) == 1; // Default to 'true' if no preference exists

        // Set font size from preferences if one is available
        float fontSize = PlayerPrefs.GetFloat("SubtitleFontSize", SettingsMenu.uninitialisedSubtitleSize);
        if (fontSize != SettingsMenu.uninitialisedSubtitleSize)
        {
			SetSubtitleFontSize(fontSize); // Apply the font size
		}
    }

    [SerializeField] private VoiceOverEnum startingSequence;

    void Start()
    {
        if (playOnStart)
        {
            PlayLevelVoiceOver();
        }
    }

    private void PlayLevelVoiceOver()
    {
        AudioManager.PlayVoiceOverWithSubtitles(startingSequence);
        Debug.Log($"Now playing {startingSequence}");
    }

    public void PlaySubtitleSequence(string sequenceName)
    {
        if (!subtitlesEnabled)
        {
            Debug.Log("Subtitles are disabled globally.");
            return; // Don't play any subtitles if they are globally disabled
        }

        if (activeSequences.Contains(sequenceName))
        {
            Debug.Log($"Sequence '{sequenceName}' is already in progress. Skipping.");
            return;
        }

        SubtitleSequence sequence = GetSubtitleSequence(sequenceName);
        if (sequence.subtitles == null || sequence.subtitles.Length == 0)
        {
            Debug.LogWarning($"Subtitle sequence '{sequenceName}' not found or empty!");
            return;
        }

        activeSequences.Add(sequenceName);

        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
            subtitleCoroutine = null;
        }
        subtitleCoroutine = StartCoroutine(PlaySequence(sequence));
    }

    private IEnumerator PlaySequence(SubtitleSequence sequence)
    {
        string sequenceName = sequence.sequenceName;
        if (!sequenceIndexTracker.ContainsKey(sequenceName))
        {
            sequenceIndexTracker[sequenceName] = 0;
        }

        float subtitleEndPosition = 0f;

        for (int i = sequenceIndexTracker[sequenceName]; i < sequence.subtitles.Length; i++)
        {
            subtitleText.text = sequence.subtitles[i].text;

            subtitleEndPosition += sequence.subtitles[i].duration;

            yield return new WaitUntil(() => AudioManager.Instance.VoiceOverAudioSource.time >= subtitleEndPosition || (!AudioManager.Instance.VoiceOverAudioSource.isPlaying && AudioManager.Instance.VoiceOverAudioSource.time == 0 && AudioListener.pause != true));

            subtitleText.text = "";
        }

        sequenceIndexTracker[sequenceName] = sequence.subtitles.Length;
        activeSequences.Remove(sequenceName);
    }

    private SubtitleSequence GetSubtitleSequence(string name)
    {
        foreach (var sequence in subtitleSequences)
        {
            if (sequence.sequenceName == name)
            {
                return sequence;
            }
        }
        return new SubtitleSequence();
    }

    // Set subtitle enable/disable and save the preference to PlayerPrefs
    public void SetSubtitlesEnabled(bool enabled)
    {
        // If disabling subtitles, immediately stop the current subtitle sequence
        if (!enabled)
        {
            StopSubtitles(); // Stop the subtitle coroutine
            ClearSubtitleText(); // Clear any existing subtitle text immediately
        }

        subtitlesEnabled = enabled;
        PlayerPrefs.SetInt("SubtitlesEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void ForceStopSubtitles()
    {
        Instance.StopSubtitles(); // This already clears the coroutine and text
        Debug.Log("Subtitles forcefully stopped.");
    }

    private void StopSubtitles()
    {
        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
            subtitleCoroutine = null;  // Clear the reference to the coroutine
        }

        // Ensure subtitle text is cleared immediately
        ClearSubtitleText();
    }

    private void ClearSubtitleText()
    {
        subtitleText.text = ""; // Clear the subtitle text immediately
    }

    public void SetSubtitleFontSize(float size)
    {
        print("font size chanign to: " + size);
        if (subtitleText != null)
        {
            subtitleText.fontSize = size;
        }
    }
}
