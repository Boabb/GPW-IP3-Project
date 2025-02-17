using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundEffect
{
    LandOnWood,
    WalkWood,
    Vent,
    WoodenFootsteps,
    WoodenJump,
    WoodenScrape,
    SuzanneExhale,
    SuzanneExert
}
public enum VoiceOver
{
    Wallenberg1,
    Wallenberg2,
    Wallenberg3,
    EmbroideryPickup
}
public enum BackgroundMusic
{
    FirstRoomSong,
    InVentSong,
    OutOfVentSong
}

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
    public List<MultipleInstanceAudio> AllSounds;

    [Header("Change (Drag and Drop)")]
    [Header("Audio Sources")]
    [SerializeField] private AudioSource MusicAudioSource;
    [SerializeField] private AudioSource VoiceOverAudioSource;
    [Header("Audio Mixers")]
    [SerializeField] private AudioMixerGroup SoundEffectsMixerGroup;
    [SerializeField] private AudioMixerGroup MusicMixerGroup;
    [SerializeField] private AudioMixerGroup VoiceOverMixerGroup;
    public MultipleInstanceAudio[] SoundEffects = new MultipleInstanceAudio[Enum.GetNames(typeof(SoundEffect)).Length];
    public SingleInstanceAudio[] BackgroundMusics = new SingleInstanceAudio[Enum.GetNames(typeof(BackgroundMusic)).Length];
    public SingleInstanceAudio[] VoiceOvers = new SingleInstanceAudio[Enum.GetNames(typeof(VoiceOver)).Length];

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
        if(Enum.GetNames(typeof(SoundEffect)).Length != SoundEffects.Length)
            SoundEffects = new MultipleInstanceAudio[Enum.GetNames(typeof(SoundEffect)).Length];

        if (Enum.GetNames(typeof(BackgroundMusic)).Length != BackgroundMusics.Length)
            BackgroundMusics = new SingleInstanceAudio[Enum.GetNames(typeof(BackgroundMusic)).Length];

        if (Enum.GetNames(typeof(VoiceOver)).Length != VoiceOvers.Length)
            VoiceOvers = new SingleInstanceAudio[Enum.GetNames(typeof(VoiceOver)).Length];

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
        string[] bGMsAsStrings = Enum.GetNames(typeof(BackgroundMusic));
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
    /// Plays the <seealso cref="SoundEffect"/> of type <paramref name="soundEffect"/> on its respective AudioSource (if there is one) or it will use the <seealso cref="MasterAudioSource"/> (if it exists)
    /// </summary>
    /// <param name="soundEffect">The <seealso cref="SoundEffect"/> to be played (Look at <seealso cref="SoundEffect"/> Enum to find the index).</param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>

    public static void PlaySoundEffect(SoundEffect soundEffect, float volume = 1.0f)
    {
        if (CheckIfValidAudioSource(Instance.SoundEffects[(int)soundEffect].audioSource))
        {
            switch (soundEffect)
            {
                case SoundEffect.WoodenScrape:
                case SoundEffect.WoodenFootsteps:
                    Instance.SoundEffects[(int)soundEffect].audioSource.clip = Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length - 1)];
                    Instance.SoundEffects[(int)soundEffect].audioSource.Play();
                    break;

                default:
                    Instance.SoundEffects[(int)soundEffect].audioSource.PlayOneShot(Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length - 1)], volume);
                    break;
            }
        }
        else
        {
            PlaySound(Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length)], volume);
        }
    }
    /// <summary>
    /// Play(s)OneShot of the <seealso cref="SoundEffect"/> of type <paramref name="soundEffect"/> on its respective AudioSource (if there is one) or it will use the <seealso cref="MasterAudioSource"/> (if it exists)
    /// </summary>
    /// <param name="soundEffect">The <seealso cref="SoundEffect"/> to be played (Look at <seealso cref="SoundEffect"/> Enum to find the index).</param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>

    public static void PlaySoundEffectOneShot(SoundEffect soundEffect, float volume = 1.0f)
    {
        if (CheckIfValidAudioSource(Instance.SoundEffects[(int)soundEffect].audioSource))
        {
            Instance.SoundEffects[(int)soundEffect].audioSource.PlayOneShot(Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length - 1)], volume);
        }
        else
        {
            PlaySound(Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length)], volume);
        }
    }

    private static bool CheckIfValidAudioSource(AudioSource audioSource)
    {
        bool valid = true;
        if(audioSource == null)
        {
            Debug.LogWarning($"Audio Source is null, Audio Source is invalid.");
            valid = false;
        }
        else if (audioSource.outputAudioMixerGroup == null)
        {
            Debug.LogWarning($"Audio Mixer Group of {audioSource.name} is null, Audio Source is invalid.");
            valid = false;
        }
        return valid;
    }

    /// <summary>
    /// Stops the <seealso cref="SoundEffect"/> of type <paramref name="soundEffect"/> on the AudioSource of the <seealso cref="SoundEffect"/> (if there is an AudioSource attached to it), otherwise it will <seealso cref="MasterAudioSource"/> (if it exists)
    /// </summary>
    /// <param name="soundEffect">The <seealso cref="SoundEffect"/> to be played (Look at <seealso cref="SoundEffect"/> Enum to find the index).</param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>

    public static void StopSoundEffect(SoundEffect soundEffect, float volume = 1.0f)
    {

        switch (soundEffect)
        {
            case SoundEffect.WoodenScrape:
            case SoundEffect.WoodenFootsteps:
                if (Instance.SoundEffects[(int)soundEffect].audioSource != null)
                {
                    if(Instance.SoundEffects[(int)soundEffect].audioSource.isPlaying)
                    {
                        Instance.SoundEffects[(int)soundEffect].audioSource.Stop();
                    }
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
    public static void PlayBackgroundMusic(BackgroundMusic musicType, float volume = 1.0f)
    {
        if (Instance.MusicAudioSource.isPlaying && Instance.BackgroundMusics[(int)musicType].clip != Instance.MusicAudioSource.clip)
        {
            Instance.MusicAudioSource.clip = Instance.BackgroundMusics[(int)musicType].clip;
            Instance.MusicAudioSource.Play();
        }
        else if (!Instance.MusicAudioSource.isPlaying)
        {
            Instance.MusicAudioSource.clip = Instance.BackgroundMusics[(int)musicType].clip;
            Instance.MusicAudioSource.Play();
        }
    }
    public void SwitchToOutOfVent()
    {
        PlayVoiceOverAudio(VoiceOver.Wallenberg2);
        PlayBackgroundMusic(BackgroundMusic.OutOfVentSong);
    }


    public void SetVolume(AudioMixer mixerGroup, float volume)
    {
        mixerGroup.SetFloat("Volume", volume);
    }
}

