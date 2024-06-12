using UnityEngine;
using TMPro;

public class BattlefieldMenu : MonoBehaviour
{
    public GameObject modeSelect;
    public GameObject battleSelectedUI;

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
    }

    public void StartBattle()
    {
        waveController.StartRound(threatLevel, battleSongID);
        Exit();
    }

    void UpdateThreatLevel()
    {
        threatLevelText.text = threatLevel.ToString();
        PlayerPrefs.SetInt("currentThreatLevel", threatLevel);
        if (threatLevel <= threatLevels.threatLevels.Length)
        {
            TTWarriorsText.text = threatLevels.threatLevels[threatLevel - 1].friendlyWarriorsAmount.ToString();
            rewardsText.text = threatLevels.threatLevels[threatLevel - 1].baseReward.ToString();
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

    public void Exit()
    {
        battleSelectedUI.SetActive(false);
        modeSelect.SetActive(true);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Exit();
    }
}
