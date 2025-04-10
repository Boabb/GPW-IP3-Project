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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private VoiceOverEnum startingSequence;

    // Start is called before the first frame update
    void Start()
    {
        PlayLevelVoiceOver();
    }

    void Update()
    {
        Debug.Log($"{AudioManager.Instance.VoiceOverAudioSource.time}");
    }

    private void PlayLevelVoiceOver()
    {
        AudioManager.PlayVoiceOverWithSubtitles(startingSequence);
        Debug.Log($"Now playing {startingSequence}");
    }

    public void PlaySubtitleSequence(string sequenceName)
    {
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

            yield return new WaitUntil(() => AudioManager.Instance.VoiceOverAudioSource.time >= subtitleEndPosition || (!AudioManager.Instance.VoiceOverAudioSource.isPlaying && AudioManager.Instance.VoiceOverAudioSource.time == 0)); // Use WaitForSecondsRealtime

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
}