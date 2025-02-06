using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public static AudioSource masterAudioSource;
    public static AudioClip BGM;
    [SerializeField] private List<AudioSourceData> soundEffects;
    [System.Serializable]
    public struct AudioSourceData
    {
        public AudioSource reference;
        public AudioMixerGroup mixerGroup;

        public AudioSourceData(AudioSource name, AudioMixerGroup type)
        {
            this.reference = name;
            this.mixerGroup = type;
        }
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
        Debug.ClearDeveloperConsole();
        foreach (var audioSrc in Transform.FindObjectsOfType<AudioSource>())
        {
            if(audioSrc.outputAudioMixerGroup == null)
            {
                Debug.LogWarning($"Audio Mixer Group of {audioSrc.name} is null, Audio Source was not added to the audioInstances List.");
                continue;
            }

            var mixerGroup = audioSrc.outputAudioMixerGroup;

            AudioSourceData newAudioInstance = new(audioSrc, mixerGroup);

            // This should be changed when we want multiple instances of sounds other than sound effects
            if (mixerGroup.name != "Sound Effects") 
            {
                foreach (AudioSourceData sound in soundEffects)
                {
                    if (sound.mixerGroup == mixerGroup)
                    {
                        Debug.LogWarning($"Audio Mixer Group of {mixerGroup} already exists in scene on AudioSource: {sound.reference}");
                    }
                }
            }
            soundEffects.Add(newAudioInstance);
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

