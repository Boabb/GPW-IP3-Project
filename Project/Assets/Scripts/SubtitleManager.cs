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
        public string sequenceName; // Name for this sequence
        public SubtitleData[] subtitles;
    }

    [Header("Subtitle Settings")]
    public SubtitleSequence[] subtitleSequences; // Array of sequences
    public TextMeshProUGUI subtitleText;

    private Dictionary<string, int> sequenceIndexTracker = new Dictionary<string, int>(); // Tracks progress for each sequence
    private HashSet<string> activeSequences = new HashSet<string>(); // Tracks sequences that are currently playing
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

    /// <summary>
    /// Starts playing a subtitle sequence by name.
    /// </summary>
    public void PlaySubtitleSequence(string sequenceName)
    {
        // Check if sequence is already playing
        if (activeSequences.Contains(sequenceName))
        {
            Debug.Log($"Sequence '{sequenceName}' is already in progress. Skipping.");
            return; // Skip if sequence is currently playing
        }

        SubtitleSequence sequence = GetSubtitleSequence(sequenceName);
        if (sequence.subtitles == null || sequence.subtitles.Length == 0)
        {
            Debug.LogWarning($"Subtitle sequence '{sequenceName}' not found or empty!");
            return;
        }

        // Add the sequence to active sequences
        activeSequences.Add(sequenceName);

        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
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

        for (int i = sequenceIndexTracker[sequenceName]; i < sequence.subtitles.Length; i++)
        {
            subtitleText.text = sequence.subtitles[i].text;
            yield return new WaitForSeconds(sequence.subtitles[i].duration);
            subtitleText.text = "";
        }

        sequenceIndexTracker[sequenceName] = sequence.subtitles.Length; // Mark as fully played

        // Once the sequence is finished, remove it from active sequences
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
        return new SubtitleSequence(); // Return empty if not found
    }
}
