using UnityEngine;

public class ThreatLevels : MonoBehaviour
{
    [System.Serializable]
    public class ThreatLevel
    {
        [Header("Creature Spawn Setup")]
        public string wave;
        public int friendlyWarriorsAmount;

        [Header("Rewards")]
        public int baseReward;
        public int timeBonusReward;
        public int timeLimitForBonusReward;
    }

    public ThreatLevel[] threatLevels;

    public ThreatLevel GetThreatLevel(int id)
    {
        return threatLevels[id];
    }
}
