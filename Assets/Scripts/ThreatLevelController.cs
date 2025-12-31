using UnityEngine;
using TMPro;

public class ThreatLevelController : MonoBehaviour
{
    public float threatProgressionValue;

    public GameObject newThreatLevelElement;
    public TMP_Text newThreatLevelUnlockTMP;

    void Start()
    {
        if (GameState.Instance.progressionData.threatProgressionValue <= 0f)
        {
            threatProgressionValue = 3.6f; // Starting value
            GameState.Instance.progressionData.threatProgressionValue = threatProgressionValue;
            GameState.Instance.progressionData.highestThreatValue = threatProgressionValue;
        }
        else
        {
            threatProgressionValue = GameState.Instance.progressionData.threatProgressionValue;
        }
    }

    public void SetThreatProgressionValue(bool wonBattle, int threatLevel, int totalRewards, int maxRewards)
    {
        float initialProgressionValue = threatProgressionValue;

        if (wonBattle)
        {
            float successFactor = ((float)totalRewards / maxRewards >= 0.87f) ? 0.65f : 0.42f;
            threatProgressionValue += successFactor * GetThreatFactor(threatLevel);
        }
        else
        {
            int diff = Mathf.FloorToInt(GameState.Instance.progressionData.threatProgressionValue) - threatLevel;
            if (diff == 0) threatProgressionValue -= 0.125f;
            else if (diff == 1) threatProgressionValue -= 0.175f;
            else threatProgressionValue -= 0.225f;

            // You can't lower the threat level more than 2 stages
            float highestAchievedValue = GameState.Instance.progressionData.highestThreatValue;
            threatProgressionValue = Mathf.Clamp(threatProgressionValue, Mathf.FloorToInt(highestAchievedValue) - 2f, highestAchievedValue);
        }

        if (Mathf.FloorToInt(threatProgressionValue) > Mathf.FloorToInt(initialProgressionValue)) UnlockNextThreatLevel();
        else newThreatLevelElement.SetActive(false);

        GameState.Instance.progressionData.threatProgressionValue = threatProgressionValue;

        if (threatProgressionValue >= GameState.Instance.progressionData.highestThreatValue)
        {
            GameState.Instance.progressionData.highestThreatValue = threatProgressionValue; // Set new highest
        }

        GameState.Instance.SaveWorld();
    }

    private void UnlockNextThreatLevel()
    {
        newThreatLevelElement.SetActive(true);
        newThreatLevelUnlockTMP.text = "Threat Level " + Mathf.FloorToInt(threatProgressionValue) + " unlocked!";
    }

    private float GetThreatFactor(int threatLevel)
    {
        int diff = Mathf.FloorToInt(GameState.Instance.progressionData.threatProgressionValue) - threatLevel;
        if (diff <= 0) return 1f;
        else if (diff == 1) return 0.375f;
        else if (diff == 2) return 0.21f;
        else if (diff == 3) return 0.125f;
        return 0.05f;
    }
}