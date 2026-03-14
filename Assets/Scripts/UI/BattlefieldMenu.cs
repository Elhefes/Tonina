using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattlefieldMenu : MonoBehaviour
{
    public GameObject modeSelect;
    public GameObject battleSelectedUI;
    public GameObject buildSelectedUI;

    public GameObject weaponSelectionMenu;

    private int threatLevel;
    public ThreatLevels threatLevels;
    public TMP_Text jadeaWarriorsText;
    public TMP_Text rewardsText;

    public GameObject musicSelectionObject;
    public GameObject randomMusicSelectionObject;

    private bool battleSongRandomized;
    private int battleSongID;
    public Image randomMusicToggleImage;
    public Sprite checkMarkOn;
    public Sprite checkMarkOff;
    public TMP_Text threatLevelText;
    public TMP_Text chosenSong;
    public TMP_Text maizeAmount;

    public TMP_Text placeablesMaxText;
    public TMP_Text placeablesUnlockText;

    public Button startBattleButton;

    int maxThreatLevel = 24;

    public WaveController waveController;
    public CutsceneCamera cutsceneCamera;

    private void OnEnable()
    {
        threatLevel = PlayerPrefs.GetInt("currentThreatLevel", 1);
        battleSongID = PlayerPrefs.GetInt("battleSongID", 0);
        battleSongRandomized = PlayerPrefs.GetInt("battleSongRandomized", 1) == 1 ? true : false;
        SetMusicSelectionVisuals(battleSongRandomized);
        maizeAmount.text = GameState.Instance.progressionData.maizeProductionLevel.ToString();
        UpdateThreatLevel();
        UpdatePlaceablesInfoTexts();
    }

    public void SelectBattle()
    {
        modeSelect.SetActive(false);
        battleSelectedUI.SetActive(true);

        // This is used to update the custom weapon selection from WeaponSelection's OnEnable function
        if (!weaponSelectionMenu.activeInHierarchy)
        {
            weaponSelectionMenu.SetActive(true);
            weaponSelectionMenu.SetActive(false);
        }
    }

    public void SelectBuild()
    {
        modeSelect.SetActive(false);
        buildSelectedUI.SetActive(true);
    }

    public void StartBattle()
    {
        if (PlayerPrefs.GetInt("battleSongRandomized", 1) == 1)
        {
            waveController.StartRound(threatLevel, Random.Range(0, waveController.musicPlayer.battleSongs.Count));
        }
        else
        {
            waveController.StartRound(threatLevel, battleSongID);
        }

        if (waveController.statsController.battlesWon < 1)
        {
            cutsceneCamera.StartAnimation("FirstBattle");
            cutsceneCamera.mouseLook.distanceFromObject = 15f;
        }

        gameObject.SetActive(false);
    }

    private void UpdateThreatLevel()
    {
        threatLevelText.text = threatLevel.ToString();
        PlayerPrefs.SetInt("currentThreatLevel", threatLevel);
        if (threatLevel <= threatLevels.threatLevels.Length && threatLevel <= Mathf.FloorToInt(waveController.threatLevelController.threatProgressionValue))
        {
            jadeaWarriorsText.text = threatLevels.threatLevels[threatLevel - 1].friendlyWarriorsAmount.ToString();
            rewardsText.text = threatLevels.threatLevels[threatLevel - 1].minReward.ToString() + " - " + threatLevels.threatLevels[threatLevel - 1].maxReward.ToString();
            startBattleButton.interactable = true;
        }
        else
        {
            jadeaWarriorsText.text = "-";
            rewardsText.text = "-";
            startBattleButton.interactable = false;
        }
    }

    private void UpdatePlaceablesInfoTexts()
    {
        placeablesMaxText.text = "You can place " + (3 + GameState.Instance.progressionData.extraPyramidFloorsBuilt * 2) + " in total!";
        if (GameState.Instance.progressionData.extraPyramidFloorsBuilt > 4) placeablesUnlockText.gameObject.SetActive(false);
        else placeablesUnlockText.gameObject.SetActive(true);
    }

    public void ToggleRandomBattleMusic()
    {
        if (PlayerPrefs.GetInt("battleSongRandomized", 1) == 1)
        {
            PlayerPrefs.SetInt("battleSongRandomized", 0);
        }
        else
        {
            PlayerPrefs.SetInt("battleSongRandomized", 1);
        }
        SetMusicSelectionVisuals(PlayerPrefs.GetInt("battleSongRandomized", 1) == 1);
    }

    private void SetMusicSelectionVisuals(bool musicIsRandomized)
    {
        if (musicIsRandomized)
        {
            musicSelectionObject.SetActive(false);
            randomMusicSelectionObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            randomMusicToggleImage.sprite = checkMarkOn;
        }
        else
        {
            musicSelectionObject.SetActive(true);
            randomMusicSelectionObject.transform.localPosition = new Vector3(0f, -70f, 0f);
            randomMusicToggleImage.sprite = checkMarkOff;
            UpdateBattleSong();
        }
    }

    private void UpdateBattleSong()
    {
        chosenSong.text = waveController.musicPlayer.battleSongs[battleSongID].name;
        PlayerPrefs.SetInt("battleSongID", battleSongID);
    }

    public void NextThreatLevel()
    {
        if (threatLevel < maxThreatLevel)
        {
            threatLevel++;
            UpdateThreatLevel();
        }
    }

    public void PreviousThreatLevel()
    {
        if (threatLevel > 1)
        {
            threatLevel--;
            UpdateThreatLevel();
        }
    }

    public void NextBattleSong()
    {
        if (battleSongID < waveController.musicPlayer.battleSongs.Count - 1) battleSongID++;
        else battleSongID = 0;
        UpdateBattleSong();
    }

    public void PreviousBattleSong()
    {
        if (battleSongID > 0) battleSongID--;
        else battleSongID = waveController.musicPlayer.battleSongs.Count - 1;
        UpdateBattleSong();
    }

    private void OnDisable()
    {
        battleSelectedUI.SetActive(false);
        buildSelectedUI.SetActive(false);
        modeSelect.SetActive(true);
        gameObject.SetActive(false);
    }
}
