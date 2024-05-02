using UnityEngine;
using TMPro;

public class BattlefieldMenu : MonoBehaviour
{
    public GameObject modeSelect;
    public GameObject battleSelectedUI;

    public int threatLevel;
    public TMP_Text threatLevelText;

    int maxThreatLevel = 25;

    public WaveController waveController;

    private void Start()
    {
        threatLevel = PlayerPrefs.GetInt("currentThreatLevel", 1);
        UpdateThreatLevel();
    }

    public void SelectBattle()
    {
        modeSelect.SetActive(false);
        battleSelectedUI.SetActive(true);
    }

    public void StartBattle()
    {
        waveController.StartRound(threatLevel);
        Exit();
    }

    void UpdateThreatLevel()
    {
        threatLevelText.text = threatLevel.ToString();
        PlayerPrefs.SetInt("currentThreatLevel", threatLevel);
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
