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

    [Header("Subtitles")]
    public Toggle subtitlesToggle;
    public Slider subtitleFontSizeSlider;

    private SubtitleManager subtitleManager; // Declare SubtitleManager reference

    void Start()
    {
        // Initialize the audio manager
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

        // Add listeners for the volume sliders (without saving to PlayerPrefs)
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        voiceSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);

        LogPlayerPrefs();

        // Initialize SubtitleManager
        subtitleManager = SubtitleManager.Instance; // Ensure SubtitleManager is referenced here

        // Load and set the subtitles toggle state
        bool subtitlesEnabled = PlayerPrefs.GetInt("SubtitlesEnabled", 1) == 1;
        subtitlesToggle.isOn = subtitlesEnabled;
        subtitlesToggle.onValueChanged.AddListener(OnSubtitlesToggle);

        // Load and set subtitle font size
        float fontSize = PlayerPrefs.GetFloat("SubtitleFontSize", 24f);
        subtitleFontSizeSlider.value = fontSize;
        subtitleManager.SetSubtitleFontSize(fontSize);  // Update subtitle manager with the saved size
        subtitleFontSizeSlider.onValueChanged.AddListener(OnSubtitleFontSizeChanged);

        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return null; // Wait one frame
        gameObject.SetActive(false);
    }

    // Volume slider value change handlers (without saving to PlayerPrefs)
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

    // Save changes to PlayerPrefs when the save button is pressed
    public void SaveChanges()
    {
        PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", currentSFXVolume);
        PlayerPrefs.SetFloat("VoiceVolume", currentVoiceVolume);
        PlayerPrefs.SetInt("SubtitlesEnabled", subtitlesToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("SubtitleFontSize", subtitleFontSizeSlider.value);
        PlayerPrefs.Save();
        Debug.Log("Settings saved!");
    }

    // Revert to the previously saved settings
    public void RevertSettings()
    {
        // Revert Volume Sliders
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

        // Revert Subtitles Toggle
        bool subtitlesEnabled = PlayerPrefs.GetInt("SubtitlesEnabled", 1) == 1;
        subtitlesToggle.isOn = subtitlesEnabled;

        // Revert subtitle font size
        float savedFontSize = PlayerPrefs.GetFloat("SubtitleFontSize", 24f);
        subtitleFontSizeSlider.SetValueWithoutNotify(savedFontSize);
        subtitleManager.SetSubtitleFontSize(savedFontSize);

        Debug.Log("Settings reverted!");
    }

    // Log PlayerPrefs values for debugging
    void LogPlayerPrefs()
    {
        Debug.Log("Music: " + PlayerPrefs.GetFloat("MusicVolume", -1));
        Debug.Log("SFX: " + PlayerPrefs.GetFloat("SFXVolume", -1));
        Debug.Log("Voice: " + PlayerPrefs.GetFloat("VoiceVolume", -1));
    }

    // Called when the subtitles toggle is changed
    void OnSubtitlesToggle(bool isOn)
    {
        subtitleManager.SetSubtitlesEnabled(isOn);
        // Removed saving to PlayerPrefs here
    }

    // Called when the subtitle font size slider is changed
    void OnSubtitleFontSizeChanged(float size)
    {
        subtitleManager.SetSubtitleFontSize(size);
        // Removed saving to PlayerPrefs here
    }
}
