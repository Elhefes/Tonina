using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioSource musicController;
    public Button musicToggleButton;
    public Sprite musicIcon;
    public Sprite muteMusicIcon;
    private bool musicOn = true;
    public Button soundToggleButton;
    public Sprite soundIcon;
    public Sprite muteSoundIcon;
    private bool soundsOn = true;

    private void Awake()
    {
        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        musicController.volume = savedVolume;

        int musicMuted = PlayerPrefs.GetInt("musicMuted", 0);
        if (musicMuted == 1)
        {
            SetMusic(false);
        }
        else
        {
            SetMusic(true);
        }

        int soundMuted = PlayerPrefs.GetInt("soundMuted", 0);
        if (soundMuted == 1)
        {
            SetSounds(false);
        }
        else
        {
            SetSounds(true);
        }
    }

    public void SetMusic(bool value)
    {
        musicOn = value;
        if (musicOn)
        {
            musicToggleButton.image.sprite = musicIcon;
            PlayerPrefs.SetInt("musicMuted", 0);
            musicController.volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        }
        else
        {
            PlayerPrefs.SetInt("musicMuted", 1);
            musicToggleButton.image.sprite = muteMusicIcon;
            musicController.volume = 0;
        }
    }

    void SetSounds(bool value)
    {
        soundsOn = value;
        if (soundsOn)
        {
            soundToggleButton.image.sprite = soundIcon;
            PlayerPrefs.SetInt("soundMuted", 0);
            PlayerPrefs.SetFloat("soundVolume", PlayerPrefs.GetFloat("soundSliderValue", 0.5f));
        }
        else
        {
            PlayerPrefs.SetInt("soundMuted", 1);
            soundToggleButton.image.sprite = muteSoundIcon;
            PlayerPrefs.SetFloat("soundVolume", 0f);
        }
    }

    public void ToggleMusic()
    {
        SetMusic(!musicOn);
    }

    public void ToggleSounds()
    {
        SetSounds(!soundsOn);
    }

    public void ChangeMusicVolume(Slider musicVolumeSlider)
    {
        SetMusic(true);
        PlayerPrefs.SetInt("musicMuted", 0);
        float volume = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("musicVolume", volume);
        musicController.volume = volume;
    }

    public void ChangeSoundVolume(Slider soundVolumeSlider)
    {
        SetSounds(true);
        PlayerPrefs.SetInt("soundMuted", 0);
        float volume = soundVolumeSlider.value;
        PlayerPrefs.SetFloat("soundVolume", volume);
        PlayerPrefs.SetFloat("soundSliderValue", volume);
    }
}
