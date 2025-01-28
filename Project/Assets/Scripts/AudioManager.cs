using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioSource masterAudioSource;
    public static AudioMixer audioMixer;
    public static AudioClip BGM;

    private void Start()
    {
        masterAudioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip audioClip, float volume = 1.0f)
    {
        masterAudioSource.PlayOneShot(audioClip, volume);
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
