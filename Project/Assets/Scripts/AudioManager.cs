using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundEffectEnum
{
    LandOnWood,
    WalkWood,
    Vent,
    WoodenFootsteps,
    WoodenJump,
    WoodenScrape,
    SuzanneExhale,
    SuzanneExert,
    LockedDoor,
    ElevatorDoorOpen,
    ElevatorDoorClose,
    Marching
}
public enum VoiceOverEnum
{
    //level testimony
    Level1Track1,
    Level1Track2,
    Level1Track3,
    Level1Track4,
    Level2Track1And2,
    Level2Track3,
    Level2Track4,
    Level2Track5,
    Level3Track1,
    Level3Track2,
    Level3Track3,
    Level3Track4,

    //end montage testimony
    EndingMontageTrack1,
    EndingMontageTrack2,
    EndingMontageTrack3,
    EndingMontageTrack4,

    //collectable testimony
    BookCollectable,
    BrokenToyCollectable,
    FoodCardCollectable,
    EmbroideredRoseCollectable
}
public enum BackgroundMusicEnum
{
    FirstRoomSong,
    InVentSong,
    OutOfVentSong
}

[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public static AudioSource MasterAudioSource;
    public static AudioClip currentBGM;
    [Header("Do Not Change")]
    /// <summary>
    /// This should not be changed in the Inspector
    /// </summary>
    public List<AudioInstance> AllSounds;

    [Header("Change (Drag and Drop)")]
    [Header("Audio Sources")]
    [SerializeField] public AudioSource MusicAudioSource;
    [SerializeField] public AudioSource VoiceOverAudioSource;
    [Header("Audio Mixers")]
    [SerializeField] private AudioMixerGroup SoundEffectsMixerGroup;
    [SerializeField] private AudioMixerGroup MusicMixerGroup;
    [SerializeField] private AudioMixerGroup VoiceOverMixerGroup;
    [SerializeField] private SubtitleManager SubtitleManager;  // Reference to the SubtitleManager

    public SoundEffect[] SoundEffects = new SoundEffect[Enum.GetNames(typeof(SoundEffectEnum)).Length];
    public BackgroundMusic[] BackgroundMusics = new BackgroundMusic[Enum.GetNames(typeof(BackgroundMusicEnum)).Length];
    public VoiceOver[] VoiceOvers = new VoiceOver[Enum.GetNames(typeof(VoiceOverEnum)).Length];

    private Stack<AudioClip> VoiceOverQueue = new Stack<AudioClip>();
    private VoiceOverEnum currentVoiceOverAudio;
    private bool isPlaying;

    private const string MUSIC_VOLUME_PARAM = "MusicVolume";
    private const string SFX_VOLUME_PARAM = "SFXVolume";
    private const string VO_VOLUME_PARAM = "VoiceVolume";

    #region

    [System.Serializable]
    public struct AudioInstance
    {
        public string name;
        public AudioSource reference;
        public AudioMixerGroup mixerGroup;

        public AudioInstance(AudioSource audioSrc, AudioMixerGroup mixerGrp)
        {
            this.reference = audioSrc;
            this.mixerGroup = mixerGrp;
            this.name = String.Concat(this.reference.name, $" ({mixerGroup.name})");
        }
    }
    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip[] clips;
        public AudioSource audioSource;
        public AudioMixerGroup mixerGroup;
    }
    [System.Serializable]
    public struct BackgroundMusic
    {
        public string name;
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
    }
    [System.Serializable]
    public struct VoiceOver
    {
        public string name;
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public float progress;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        OnSceneRefresh();
    }
#endif
    private void OnSceneRefresh()
    {
        if (AllSounds != null)
        {
            AllSounds.Clear();
        }

        var noOfSFXsInInspector = Enum.GetNames(typeof(SoundEffectEnum)).Length;
        var noOfBGMsInInspector = Enum.GetNames(typeof(BackgroundMusicEnum)).Length;
        var noOfVOsInInspector = Enum.GetNames(typeof(VoiceOverEnum)).Length;

        //if (Enum.GetNames(typeof(SoundEffectEnum)).Length != SoundEffects.Length)
        //{
        //    var temp = new SoundEffect[SoundEffects.Length];
        //    SoundEffects.CopyTo(temp, noOfSFXsInInspector);
        //    SoundEffects = temp;

        //}

        //if (noOfBGMsInInspector != BackgroundMusics.Length)
        //    BackgroundMusics = new BackgroundMusic[noOfBGMsInInspector];

        //if (Enum.GetNames(typeof(VoiceOverEnum)).Length != VoiceOvers.Length)
        //    VoiceOvers = new VoiceOver[Enum.GetNames(typeof(VoiceOverEnum)).Length];

        Debug.ClearDeveloperConsole();
        foreach (var audioSrc in FindObjectsOfType<AudioSource>())
        {
            if (audioSrc.outputAudioMixerGroup == null)
            {
                Debug.LogWarning($"Audio Mixer Group of {audioSrc.name} is null, Audio Source was not added to the audioInstances List.");
                continue;
            }

            var mixerGroup = audioSrc.outputAudioMixerGroup;

            AudioInstance newAudioInstance = new(audioSrc, mixerGroup);

            // This should be changed when we want multiple instances of sounds other than sound effects
            if (mixerGroup.name != "Sound Effects")
            {
                //foreach (SingleInstanceAudio sound in AllSounds)
                //{
                //    if (sound.mixerGroup == mixerGroup)
                //    {
                //        Debug.LogWarning($"Audio Mixer Group of {mixerGroup} already exists in scene on AudioSource: {sound.audioSource}");
                //    }
                //}
            }
            else
            {
                int i = 0;
                foreach (var temp in Enum.GetNames(typeof(SoundEffectEnum)))
                {
                    if (audioSrc.clip != null)
                    {
                        if (string.Equals(temp, audioSrc.clip.name, StringComparison.OrdinalIgnoreCase))
                        {
                            SoundEffects[i].audioSource = audioSrc;
                        }
                    }
                    i++;

                }
            }

            AllSounds.Add(newAudioInstance);
        }
        string[] voiceOversAsStrings = Enum.GetNames(typeof(VoiceOverEnum));
        string[] bGMsAsStrings = Enum.GetNames(typeof(BackgroundMusicEnum));
        string[] soundEffectsAsStrings = Enum.GetNames(typeof(SoundEffectEnum));

        for (int i = 0; i < VoiceOvers.Length; i++)
        {
            VoiceOvers[i].mixerGroup = VoiceOverMixerGroup;
            VoiceOvers[i].name = voiceOversAsStrings[i];
        }
        for (int i = 0; i < BackgroundMusics.Length; i++)
        {
            BackgroundMusics[i].mixerGroup = MusicMixerGroup;
            BackgroundMusics[i].name = bGMsAsStrings[i];
        }
        for (int i = 0; i < SoundEffects.Length; i++)
        {
            SoundEffects[i].mixerGroup = SoundEffectsMixerGroup;
            SoundEffects[i].name = soundEffectsAsStrings[i];
        }

    }
    #endregion


    private void Awake()
    {
        Instance = this;
        MasterAudioSource = GetComponent<AudioSource>();
        SubtitleManager = GetComponentInChildren<SubtitleManager>();
    }

    private void Start()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Debug.LogWarning("More than one AudioManager Detected! Please find and remove the extra one!");
        }

        Instance.VoiceOverQueue.Clear();

        //// Load volume from PlayerPrefs
        //float musicVol = PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM, 0.75f);
        //float sfxVol = PlayerPrefs.GetFloat(SFX_VOLUME_PARAM, 0.75f);
        //float voVol = PlayerPrefs.GetFloat(VO_VOLUME_PARAM, 0.75f);

        //ApplySavedVolumes();

        //// Apply the loaded volume settings directly using SetMixerVolume
        //SetMixerVolume(MusicMixerGroup.audioMixer, MUSIC_VOLUME_PARAM, musicVol);
        //SetMixerVolume(SoundEffectsMixerGroup.audioMixer, SFX_VOLUME_PARAM, sfxVol);
        //SetMixerVolume(VoiceOverMixerGroup.audioMixer, VO_VOLUME_PARAM, voVol);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM, value);
        SetMixerVolume(MusicMixerGroup.audioMixer, MUSIC_VOLUME_PARAM, value);
    }
    public void SetSoundEffectsVolume(float value)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_PARAM, value);
        SetMixerVolume(SoundEffectsMixerGroup.audioMixer, SFX_VOLUME_PARAM, value);
    }
    public void SetVoiceOverVolume(float value)
    {
        PlayerPrefs.SetFloat(VO_VOLUME_PARAM, value);
        SetMixerVolume(VoiceOverMixerGroup.audioMixer, VO_VOLUME_PARAM, value);
    }

    public void SetMixerVolume(AudioMixer mixer, string param, float value)
    {
        if (value <= 0.0001f) // Protect against zero
        {
            Debug.Log("Muted");
            mixer.SetFloat(param, -80f); // Practically silent
        }
        else
        {
            mixer.SetFloat(param, Mathf.Log10(value) * 20f);
        }
    }

    public void ApplySavedVolumes()
    {
        UpdateVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        UpdateVolume("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        UpdateVolume("VoiceVolume", PlayerPrefs.GetFloat("VoiceVolume", 0.5f));
    }

    public void UpdateVolume(string key, float value)
    {
        value = Mathf.Clamp(value, 0.001f, 1f);

        print("updating volume");
        switch (key)
        {
            case "MusicVolume":
                MusicAudioSource.volume = value;  // Adjust the MusicAudioSource volume
                break;
            case "SFXVolume":
                // Assuming there is an array or similar for sound effect sources, you may want to update them here.
                // If you have multiple SFX audio sources, you would loop through and set their volume.
                break;
            case "VoiceVolume":
                VoiceOverAudioSource.volume = value;  // Ensure this is actually modifying the VoiceOverAudioSource's volume
                break;
            default:
                Debug.LogWarning($"Unknown volume key: {key}");
                break;
        }
    }

    public static void PlaySound(AudioClip audioClip, float volume = 1.0f)
    {
        MasterAudioSource.PlayOneShot(audioClip, volume);
    }

    /// <summary>
    /// Plays the <seealso cref="SoundEffectEnum"/> Audio of type <paramref name="soundEffect"/> on its respective AudioSource (if there is one) or it will use the <seealso cref="MasterAudioSource"/> (if it exists)
    /// </summary>
    /// <param name="soundEffect">The <seealso cref="SoundEffectEnum"/> to be played (Look at <seealso cref="SoundEffectEnum"/> to find the index).</param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>

    public static void PlaySoundEffect(SoundEffectEnum soundEffect, float volume = 1.0f)
     {
        if (CheckIfValidAudioSource(Instance.SoundEffects[(int)soundEffect].audioSource))
        {
            if (!Instance.SoundEffects[(int)soundEffect].audioSource.isPlaying)
            {
                switch (soundEffect)
                {
                    case SoundEffectEnum.WoodenScrape:
                    case SoundEffectEnum.WoodenFootsteps:
                        Instance.SoundEffects[(int)soundEffect].audioSource.clip = Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length)];
                        Instance.SoundEffects[(int)soundEffect].audioSource.Play();
                        break;
                    case SoundEffectEnum.Marching:
                        Instance.SoundEffects[(int)soundEffect].audioSource.clip = Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length)];
                        Instance.SoundEffects[(int)soundEffect].audioSource.Play();
                        break;

                    default:
                        if (UnityEngine.Random.Range(0, 4) == 1)
                        {
                            if (Instance.SoundEffects[(int)soundEffect].clips.Length > 0)
                            {
                                Instance.SoundEffects[(int)soundEffect].audioSource.PlayOneShot(Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length)], volume);
                            }
                            else
                            {
                                Debug.LogWarning($"{Instance.SoundEffects[(int)soundEffect].name} is valid but Sound Effect has no clips to play!");
                            }
                        }
                        break;
                }
            }
        }
        else
        {
            switch (soundEffect)
            {
                case SoundEffectEnum.WoodenScrape:
                case SoundEffectEnum.WoodenFootsteps:
                    break;
                default:
                    if (Instance.SoundEffects[(int)soundEffect].clips.Length > 0)
                    {
                        PlaySound(Instance.SoundEffects[(int)soundEffect].clips[UnityEngine.Random.Range(0, Instance.SoundEffects[(int)soundEffect].clips.Length)], volume);
                        Debug.LogWarning($"{Instance.SoundEffects[(int)soundEffect].name} not valid. Sound Effect was played on MasterAudioSource.");
                    }
                    else
                    {
                        Debug.LogWarning($"{Instance.SoundEffects[(int)soundEffect].name} not valid. Sound Effect has no clips to play!");
                    }
                    break;
            }
        }
    }

    private static bool CheckIfValidAudioSource(AudioSource audioSource)
    {
        bool valid = true;
        if (audioSource == null)
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
    /// Stops the <seealso cref="SoundEffectEnum"/> Audio of type <paramref name="soundEffect"/> on the <seealso cref="MasterAudioSource"/> (if it exists)
    /// </summary>
    /// <param name="soundEffect">The <seealso cref="SoundEffectEnum"/> to be played (Look at <seealso cref="SoundEffectEnum"/> to find the index).</param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>

    public static void StopSoundEffect(SoundEffectEnum soundEffect, float volume = 1.0f)
    {

        switch (soundEffect)
        {
            case SoundEffectEnum.WoodenScrape:
            case SoundEffectEnum.WoodenFootsteps:
                if (Instance.SoundEffects[(int)soundEffect].audioSource != null)
                {
                    if (Instance.SoundEffects[(int)soundEffect].audioSource.isPlaying)
                    {
                        Instance.SoundEffects[(int)soundEffect].audioSource.Stop();
                    }
                }

                break;
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
    /// Plays the <seealso cref="VoiceOverEnum"/> Audio of type <paramref name="index"/> on the <seealso cref="VoiceOvers"/> respective Audio Source (if it exists)
    /// </summary>
    /// <param name="index">The <paramref name="index"/> of the Voice-Over Audio; see <seealso cref="VoiceOverEnum"/> for indexes of each audio.</param>
    public static void PlayVoiceOverAudio(VoiceOverEnum index, float volume = 1.0f)
    {
        if (Instance.VoiceOvers[(int)index].clip != null)
        {
            Debug.Log($"Starting Voice Over: {Instance.VoiceOvers[(int)index].name}");

            if (Instance.VoiceOvers[(int)index].clip != Instance.VoiceOverAudioSource.clip)
            {
                // Store current audio and its progress
                if (Instance.VoiceOverAudioSource.clip != null && Instance.VoiceOverAudioSource.isPlaying)
                {
                    int currentIndex = 0;

                    for (int i = 0; i < Instance.VoiceOvers.Length; i++)
                    {
                        if (Instance.VoiceOverAudioSource.clip == Instance.VoiceOvers[i].clip)
                        {
                            currentIndex = i;
                            break;
                        }
                    }

                    Instance.VoiceOvers[currentIndex].progress = Instance.VoiceOverAudioSource.time;
                    Instance.VoiceOverQueue.Push(Instance.VoiceOvers[currentIndex].clip);
                }

                // Start new audio
                Instance.currentVoiceOverAudio = index;
                Instance.VoiceOverAudioSource.clip = Instance.VoiceOvers[(int)index].clip;
                Instance.VoiceOverAudioSource.time = Instance.VoiceOvers[(int)index].progress;
                Instance.VoiceOverAudioSource.Play();
                Instance.isPlaying = true;

                // Resume previous audio after the new one finishes
                Instance.StartCoroutine(ResumePreviousVoiceOver());
            }
            else if(!Instance.VoiceOverAudioSource.isPlaying)
            {
                Instance.VoiceOverAudioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"Audio Clip not assigned for: {Instance.VoiceOvers[(int)index].name}. PlayVoiceOverAudio aborted");
        }
    }

    public static void PlayVoiceOverWithSubtitles(VoiceOverEnum index, float volume = -1f)
    {
        // Default to saved volume if no volume is passed
        if (volume == -1f)
        {
            volume = PlayerPrefs.GetFloat("VoiceVolume", 0.75f); // Load from PlayerPrefs if not provided
        }

        if (Instance.VoiceOvers[(int)index].clip != null)
        {
            // Play the voice-over audio
            PlayVoiceOverAudio(index, volume);  // volume should now be the value from PlayerPrefs

            // Ensure that SubtitleManager.Instance is called correctly here
            SubtitleManager.Instance.PlaySubtitleSequence(index.ToString());

            Debug.Log("VoiceOver and subtitles are playing.");
        }
        else
        {
            Debug.LogWarning($"Audio Clip not assigned for: {Instance.VoiceOvers[(int)index].name}. PlayVoiceOverWithSubtitles aborted");
        }
    }

    public static void StopVoiceOver()
    {
        if (Instance.VoiceOverAudioSource.isPlaying)
        {
            Instance.VoiceOverAudioSource.Stop();

            // Tell SubtitleManager to stop subtitles as well
            if (SubtitleManager.Instance != null)
            {
                SubtitleManager.ForceStopSubtitles();
            }
        }
    }

    private static IEnumerator ResumePreviousVoiceOver()
    {
        yield return new WaitUntil(() => Instance.VoiceOverAudioSource.isPlaying == false);

        if (Instance.VoiceOverQueue.TryPop(out var nextClip))
        {
            for (int i = 0; i < Instance.VoiceOvers.Length; i++)
            {
                if (Instance.VoiceOvers[i].clip == nextClip)
                {
                    Instance.VoiceOverAudioSource.clip = nextClip;
                    Instance.VoiceOverAudioSource.time = Instance.VoiceOvers[i].progress;
                    Instance.currentVoiceOverAudio = (VoiceOverEnum)i;
                    Instance.VoiceOverAudioSource.Play();
                    Instance.isPlaying = true;
                    Instance.StartCoroutine(ResumePreviousVoiceOver());
                    break;
                }
            }
        }
        else
        {
            Instance.isPlaying = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="musicType"></param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>
    public static void PlayBackgroundMusic(BackgroundMusicEnum musicType, float volume = 0.0f)
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
        //PlayVoiceOverAudio(VoiceOver.Wallenberg2);
        PlayBackgroundMusic(BackgroundMusicEnum.OutOfVentSong);
    }
    public void SetVolume(AudioMixer mixerGroup, float volume)
    {
        mixerGroup.SetFloat("Volume", volume);
    }

    public static AudioClip GetVoiceOverClip(VoiceOverEnum clipEnum)
    {
        var index = (int)clipEnum;

        if (index >= 0 && index < Instance.VoiceOvers.Length)
        {
            var clip = Instance.VoiceOvers[index].clip;

            if (clip != null)
                return clip;
            else
                Debug.LogWarning($"VoiceOver clip for {clipEnum} is not assigned.");
        }
        else
        {
            Debug.LogWarning($"VoiceOverEnum index {index} is out of bounds.");
        }

        return null;
    }
}

