using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioController audioController;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    public GameObject normalMenu;
    public GameObject confirmReturnHomeMenu;

    private void Awake()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.2f);
        musicVolumeSlider.onValueChanged.AddListener(delegate { audioController.ChangeMusicVolume(musicVolumeSlider); });
        soundVolumeSlider.value = PlayerPrefs.GetFloat("soundSliderValue", 0.5f);
        soundVolumeSlider.onValueChanged.AddListener(delegate { audioController.ChangeSoundVolume(soundVolumeSlider); });
    }

    private void OnEnable()
    {
        if (normalMenu != null) normalMenu.SetActive(true);
        if (confirmReturnHomeMenu != null) confirmReturnHomeMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
