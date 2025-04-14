using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class SettingsMenu : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;

    private AudioManager audioManager;

    private float currentMusicVolume;
    private float currentSFXVolume;
    private float currentVoiceVolume;

    void Start()
    {
        audioManager = AudioManager.Instance;

        // Set the sliders to saved PlayerPrefs values
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        voiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 0.5f);

        // Apply volumes immediately
        audioManager.UpdateVolume("MusicVolume", musicSlider.value);
        audioManager.UpdateVolume("SFXVolume", sfxSlider.value);
        audioManager.UpdateVolume("VoiceVolume", voiceSlider.value);

        // Store the current values
        currentMusicVolume = musicSlider.value;
        currentSFXVolume = sfxSlider.value;
        currentVoiceVolume = voiceSlider.value;

        // Add listeners (only once)
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        voiceSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);

        LogPlayerPrefs();

        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return null; // wait one frame
        gameObject.SetActive(false);
    }

    void OnMusicVolumeChanged(float value)
    {
        audioManager.UpdateVolume("MusicVolume", value);
        currentMusicVolume = value;
    }

    void OnSFXVolumeChanged(float value)
    {
        audioManager.UpdateVolume("SFXVolume", value);
        currentSFXVolume = value;
    }

    void OnVoiceVolumeChanged(float value)
    {
        audioManager.UpdateVolume("VoiceVolume", value);
        currentVoiceVolume = value;
    }

    public void SaveChanges()
    {
        PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", currentSFXVolume);
        PlayerPrefs.SetFloat("VoiceVolume", currentVoiceVolume);
        PlayerPrefs.Save();
        Debug.Log("Settings saved!");
    }

    public void RevertSettings()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        float savedVoiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 0.75f);

        musicSlider.SetValueWithoutNotify(savedMusicVolume);
        sfxSlider.SetValueWithoutNotify(savedSFXVolume);
        voiceSlider.SetValueWithoutNotify(savedVoiceVolume);

        audioManager.UpdateVolume("MusicVolume", savedMusicVolume);
        audioManager.UpdateVolume("SFXVolume", savedSFXVolume);
        audioManager.UpdateVolume("VoiceVolume", savedVoiceVolume);

        currentMusicVolume = savedMusicVolume;
        currentSFXVolume = savedSFXVolume;
        currentVoiceVolume = savedVoiceVolume;

        Debug.Log("Settings reverted!");
    }

    void LogPlayerPrefs()
    {
        Debug.Log("Music: " + PlayerPrefs.GetFloat("MusicVolume", -1));
        Debug.Log("SFX: " + PlayerPrefs.GetFloat("SFXVolume", -1));
        Debug.Log("Voice: " + PlayerPrefs.GetFloat("VoiceVolume", -1));
    }
}
