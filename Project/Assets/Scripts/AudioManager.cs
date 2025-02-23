using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public static AudioSource masterAudioSource;
    public static AudioClip currentBGM;
    [SerializeField] public AudioMixerGroup MusicMixerGroup, SoundEffectsMixerGroup, VoiceOverMixerGroup;
    [Header("Do Not Change")]
    /// <summary>
    /// This should not be changed in the Inspector
    /// </summary>
    public List<MultipleSourceAudio> AllSounds;
    [Header("Change (Drag in the AudioClips)")]
    [SerializeField] private SingleSourceAudio[] SoundEffects = new SingleSourceAudio[Enum.GetNames(typeof(SoundEffect)).Length];
    [SerializeField] private MultipleSourceAudio[] BackgroundMusics = new MultipleSourceAudio[Enum.GetNames(typeof(Music)).Length];
    [SerializeField] private MultipleSourceAudio[] VoiceOvers = new MultipleSourceAudio[Enum.GetNames(typeof(VoiceOver)).Length];
    [System.Serializable]
    public struct MultipleSourceAudio
    {
        public string name;
        public AudioSource reference;
        public AudioMixerGroup mixerGroup;

        public MultipleSourceAudio(AudioSource audioSrc, AudioMixerGroup mixerGrp)
        {
            this.reference = audioSrc;
            this.mixerGroup = mixerGrp;
            this.name = String.Concat(this.reference.name, $" ({mixerGroup.name})");
        }
    }
    [System.Serializable]
    public struct SingleSourceAudio
    {
        public string name;
        public AudioClip[] clips;
        public AudioMixerGroup mixerGroup;
    }


    public enum SoundEffect
    {
        JumpWood,
        WalkWood,
        Vent,
        WoodenFootsteps,
        WoodenJump,
        WoodenScrape
    }
    public enum VoiceOver
    {
        Wallenberg1,
        Wallenberg2,
        Wallenberg3,
        EmbroideryPickup
    }
    public enum Music
    {
        Wallenberg1,
        Wallenberg2,
        Wallenberg3,
        EmbroideryPickup
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        OnSceneRefresh();
    }
#endif
    private void OnSceneRefresh()
    {
        AllSounds.Clear();
        Debug.ClearDeveloperConsole();
        foreach (var audioSrc in FindObjectsOfType<AudioSource>())
        {
            if(audioSrc.outputAudioMixerGroup == null)
            {
                Debug.LogWarning($"Audio Mixer Group of {audioSrc.name} is null, Audio Source was not added to the audioInstances List.");
                continue;
            }

            var mixerGroup = audioSrc.outputAudioMixerGroup;

            MultipleSourceAudio newAudioInstance = new(audioSrc, mixerGroup);

            // This should be changed when we want multiple instances of sounds other than sound effects
            if (mixerGroup.name != "Sound Effects") 
            {
                foreach (MultipleSourceAudio sound in AllSounds)
                {
                    if (sound.mixerGroup == mixerGroup)
                    {
                        Debug.LogWarning($"Audio Mixer Group of {mixerGroup} already exists in scene on AudioSource: {sound.reference}");
                    }
                }
            }
            AllSounds.Add(newAudioInstance);
        }
        string[] voiceOversAsStrings = Enum.GetNames(typeof(VoiceOver));
        string[] bGMsAsStrings = Enum.GetNames(typeof(Music));
        string[] soundEffectsAsStrings = Enum.GetNames(typeof(SoundEffect));

        Array.Resize(ref VoiceOvers, voiceOversAsStrings.Length);
        Array.Resize(ref BackgroundMusics, bGMsAsStrings.Length);

        for (int i = 0; i < voiceOversAsStrings.Length; i++)
        {
            VoiceOvers[i].mixerGroup = VoiceOverMixerGroup;
            VoiceOvers[i].name = voiceOversAsStrings[i];
        }        
        for(int i = 0; i < bGMsAsStrings.Length; i++)
        {
            BackgroundMusics[i].mixerGroup = MusicMixerGroup;
            BackgroundMusics[i].name = bGMsAsStrings[i];
        }        
        for(int i = 0; i < soundEffectsAsStrings.Length; i++)
        {
            SoundEffects[i].mixerGroup = SoundEffectsMixerGroup;
            SoundEffects[i].name = soundEffectsAsStrings[i];
        }

    }

    private void Start()
    {
        Instance = this;
        masterAudioSource = GetComponent<AudioSource>();
    }


    public static void PlaySound(AudioClip audioClip, float volume = 1.0f)
    {
        masterAudioSource.PlayOneShot(audioClip, volume);
    }

    /// <summary>
    /// Plays the <seealso cref="SoundEffect"/>  Audio of type <paramref name="soundEffect"/> on the <seealso cref="masterAudioSource"/> (if it exists)
    /// </summary>
    /// <param name="soundEffect">The <seealso cref="SoundEffect"/> to be played (Look at <seealso cref="SoundEffect"/> Enum to find the index).</param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>

    public static void PlaySoundEffect(SoundEffect soundEffect, float volume = 1.0f)
    {
        masterAudioSource.PlayOneShot(Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects.Length)], volume);
    }

    public static void PlaySoundFromSource(AudioSource audioSource, AudioClip audioClip, float volume = 1.0f)
    {
        audioSource.PlayOneShot(audioClip, volume);
    }

    public static void PlaySoundAtLocation(Vector3 position, AudioClip audioClip, float volume = 1.0f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    /// <summary>
    /// Plays the <seealso cref="VoiceOver"/> Audio of type <paramref name="index"/> on the <seealso cref="VoiceOvers"/> respective Audio Source (if it exists)
    /// </summary>
    /// <param name="index">The <paramref name="index"/> of the Voice-Over Audio; see <seealso cref="VoiceOver"/> Enum for indexes of each audio.</param>
    public static void PlayVoiceOverAudio(int index)
    {
        foreach (MultipleSourceAudio voiceOver in Instance.VoiceOvers)
        {
            if (voiceOver.reference.isPlaying && index != Array.IndexOf(Instance.VoiceOvers, voiceOver))
            {
                voiceOver.reference.Pause();
            }
            else if(index == Array.IndexOf(Instance.VoiceOvers, voiceOver))
            {
                voiceOver.reference.Play();
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="newBGM"></param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>
    public static void PlayBackgroundMusic(AudioClip newBGM, float volume = 1.0f)
    {
        if(currentBGM != newBGM)
        {
            masterAudioSource.Stop();
            currentBGM = newBGM;
            masterAudioSource.clip = newBGM;
            masterAudioSource.Play();
            masterAudioSource.volume = volume;
        }
    }


    public void SetVolume(AudioMixer mixerGroup, float volume)
    {
        mixerGroup.SetFloat("Volume", volume);
    }
}

