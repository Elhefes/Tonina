using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioController audioController;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    public GameObject normalMenu;
    public GameObject confirmReturnHomeMenu;
    public GameObject confirmForfeitAttackMenu;

    public Button returnHomeButton;
    public GameObject mainMenuButton;

    public bool returnFromBuilder;

    public GameObject battleReturnConfirmText;
    public GameObject builderReturnConfirmText;

    public GameObject returnFromBattleButton;
    public GameObject returnFromBuilderButton;

    public Player player;

    public AttackModeSpawnController attackModeSpawnController;

    public void SetBuilderMode() { returnFromBuilder = true; }
    public void SetBattleMode() { returnFromBuilder = false; }

    private void OnEnable()
    {
        LoadAudioSettings();

        if (normalMenu != null) normalMenu.SetActive(true);
        if (confirmReturnHomeMenu != null) confirmReturnHomeMenu.SetActive(false);
        if (confirmForfeitAttackMenu != null) confirmForfeitAttackMenu.SetActive(false);

        if (player != null)
        {
            if (player.insideKingHouse && !player.inBuildMode)
            {
                returnHomeButton.gameObject.SetActive(false);
                mainMenuButton.SetActive(true);
                return;
            }
        }

        if (mainMenuButton != null) mainMenuButton.SetActive(false);
        if (returnHomeButton != null) returnHomeButton.gameObject.SetActive(true);

        if (battleReturnConfirmText != null && builderReturnConfirmText != null)
        {
            if (returnFromBuilder)
            {
                builderReturnConfirmText.SetActive(true);
                returnFromBuilderButton.SetActive(true);
                battleReturnConfirmText.SetActive(false);
                returnFromBattleButton.SetActive(false);
            }
            else
            {
                builderReturnConfirmText.SetActive(false);
                returnFromBuilderButton.SetActive(false);
                battleReturnConfirmText.SetActive(true);
                returnFromBattleButton.SetActive(true);
            }
        }
    }

    public void LoadAudioSettings()
    {
        // Adjust audio sliders and then mute them if they were muted before

        int musicMuted = PlayerPrefs.GetInt("musicMuted", 0);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.2f);
        musicVolumeSlider.onValueChanged.AddListener(delegate { audioController.ChangeMusicVolume(musicVolumeSlider); });
        if (musicMuted == 1)
        {
            audioController.SetMusic(false);
        }
        else
        {
            audioController.SetMusic(true);
        }

        int soundMuted = PlayerPrefs.GetInt("soundMuted", 0);
        soundVolumeSlider.value = PlayerPrefs.GetFloat("soundSliderValue", 0.5f);
        soundVolumeSlider.onValueChanged.AddListener(delegate { audioController.ChangeSoundVolume(soundVolumeSlider); });
        if (soundMuted == 1)
        {
            audioController.SetSounds(false);
        }
        else
        {
            audioController.SetSounds(true);
        }
    }

    public void ReturnToAttackSpawnSelection()
    {
        attackModeSpawnController.ReturnToSpawnSelection();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
