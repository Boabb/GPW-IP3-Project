using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("Subtitles")]
    public Toggle subtitlesToggle;
    public Slider subtitleFontSizeSlider;

    [Header("Volume Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;

    private SubtitleManager subtitleManager;

    void Start()
    {
        subtitleManager = SubtitleManager.Instance;

        bool subtitlesEnabled = PlayerPrefs.GetInt("SubtitlesEnabled", 1) == 1;
        subtitlesToggle.isOn = subtitlesEnabled;
        subtitlesToggle.onValueChanged.AddListener(OnSubtitlesToggle);

        float fontSize = PlayerPrefs.GetFloat("SubtitleFontSize", 24f);
        subtitleFontSizeSlider.value = fontSize;
        subtitleManager.SetSubtitleFontSize(fontSize);
        subtitleFontSizeSlider.onValueChanged.AddListener(OnSubtitleFontSizeChanged);

        SetupVolumeSlider(musicSlider, "MusicVolume");
        SetupVolumeSlider(sfxSlider, "SFXVolume");
        SetupVolumeSlider(voiceSlider, "VoiceVolume");

        voiceSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.Instance.UpdateVolume("VoiceVolume", value);
            PlayerPrefs.SetFloat("VoiceVolume", value);  // Save the volume setting
            PlayerPrefs.Save();
        });
    }

    void OnSubtitlesToggle(bool isOn)
    {
        subtitleManager.SetSubtitlesEnabled(isOn);
        PlayerPrefs.SetInt("SubtitlesEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnSubtitleFontSizeChanged(float size)
    {
        subtitleManager.SetSubtitleFontSize(size);
        PlayerPrefs.SetFloat("SubtitleFontSize", size);
        PlayerPrefs.Save();
    }

    void SetupVolumeSlider(Slider slider, string key)
    {
        float savedValue = PlayerPrefs.GetFloat(key, 0.75f);
        slider.value = savedValue;
        slider.onValueChanged.AddListener((value) =>
        {
            AudioManager.Instance.UpdateVolume(key, value);
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        });
    }
}
