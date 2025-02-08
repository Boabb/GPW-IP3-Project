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
    public static AudioClip BGM;
    [SerializeField] private List<MultipleSourceAudio> SoundEffects;  
    [SerializeField] private List<MultipleSourceAudio> AllSounds;
    [SerializeField] private MultipleSourceAudio[] WallenbergAudios;
    [System.Serializable]
    public struct MultipleSourceAudio
    {
        public string name;
        public AudioSource reference;
        public AudioMixerGroup mixerGroup;

        public MultipleSourceAudio(AudioSource name, AudioMixerGroup type)
        {
            this.reference = name;
            this.mixerGroup = type;
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

#if UNITY_EDITOR
    private void OnEnable()
    {
        OnValidate();
    }
    private void OnValidate()
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
    }
#endif

    private void Start()
    {
        Instance = this;
        masterAudioSource = GetComponent<AudioSource>();
    }


    public static void PlaySound(AudioClip audioClip, float volume = 1.0f)
    {
        masterAudioSource.PlayOneShot(audioClip, volume);
    }    
    
    public static void PlaySoundEffect(SoundEffect soundEffect, float volume = 1.0f)
    {
        throw new NotImplementedException();
        //masterAudioSource.PlayOneShot(Instance.soundEffects[(int)soundEffect], volume);
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
    /// Plays the Wallenberg Audio of type <paramref name="index"/> on the Wallenberg audio source (if it exists)
    /// </summary>
    /// <param name="index">The volume of the audio source (0.0 to 1.0).</param>
    public static void PlayWallenbergAudio(int index)
    {
        foreach(MultipleSourceAudio wallenbergAudio in Instance.WallenbergAudios)
        if(wallenbergAudio.reference.isPlaying && index != Array.IndexOf(Instance.WallenbergAudios, wallenbergAudio))
        {

        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="newBGM"></param>
    /// <param name="volume">The volume of the audio source (0.0 to 1.0).</param>
    public static void PlayBackgroundMusic(AudioClip newBGM, float volume = 1.0f)
    {
        if(BGM != newBGM)
        {
            masterAudioSource.Stop();
            BGM = newBGM;
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

