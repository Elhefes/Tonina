using UnityEngine;
using TMPro;

public class BattlefieldMenu : MonoBehaviour
{
    public GameObject modeSelect;
    public GameObject battleSelectedUI;
    public GameObject buildSelectedUI;

    public GameObject weaponSelectionMenu;

    private int threatLevel;
    public ThreatLevels threatLevels;
    public TMP_Text TTWarriorsText;
    public TMP_Text rewardsText;

    private int battleSongID;
    public TMP_Text threatLevelText;
    public TMP_Text chosenSong;

    int maxThreatLevel = 25;

    public WaveController waveController;

    private void Start()
    {
        threatLevel = PlayerPrefs.GetInt("currentThreatLevel", 1);
        battleSongID = PlayerPrefs.GetInt("battleSongID", 0);
        UpdateThreatLevel();
        UpdateBattleSong();
    }

    public void SelectBattle()
    {
        modeSelect.SetActive(false);
        battleSelectedUI.SetActive(true);

        // This is used to update the custom weapon selection from WeaponSelection's OnEnable function
        weaponSelectionMenu.SetActive(true);
        weaponSelectionMenu.SetActive(false);
    }

    public void SelectBuild()
    {
        modeSelect.SetActive(false);
        buildSelectedUI.SetActive(true);
    }

    public void StartBattle()
    {
        waveController.StartRound(threatLevel, battleSongID);
        gameObject.SetActive(false);
    }

    void UpdateThreatLevel()
    {
        threatLevelText.text = threatLevel.ToString();
        PlayerPrefs.SetInt("currentThreatLevel", threatLevel);
        if (threatLevel <= threatLevels.threatLevels.Length)
        {
            TTWarriorsText.text = threatLevels.threatLevels[threatLevel - 1].friendlyWarriorsAmount.ToString();
            rewardsText.text = threatLevels.threatLevels[threatLevel - 1].minReward.ToString() + " - " + threatLevels.threatLevels[threatLevel - 1].maxReward.ToString();
        }
    }

    void UpdateBattleSong()
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

    void OnDisable()
    {
        battleSelectedUI.SetActive(false);
        buildSelectedUI.SetActive(false);
        modeSelect.SetActive(true);
        gameObject.SetActive(false);
    }
}
