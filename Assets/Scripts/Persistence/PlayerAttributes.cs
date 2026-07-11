using UnityEngine;

public static class PlayerAttributes
{
    public static int VitalityHealthBuff(int vitalityLevel)
    {
        return vitalityLevel switch
        {
            1 => 20,
            2 => 40,
            3 => 70,
            4 => 100,
            5 => 150,
            _ => 0,
        };
    }
    public static int VitalityMaizeHealBuff(int vitalityLevel)
    {
        return vitalityLevel switch
        {
            1 => 10,
            2 => 20,
            3 => 30,
            4 => 40,
            5 => 60,
            _ => 0,
        };
    }
    public static float AgilitySpeedBuff(int agilityLevel)
    {
        return agilityLevel switch
        {
            1 => 0.1f,
            2 => 0.2f,
            3 => 0.35f,
            4 => 0.5f,
            5 => 0.65f,
            _ => 0,
        };
    }
    public static float AgilityStaminaBuff(int agilityLevel)
    {
        return agilityLevel switch
        {
            1 => 1f,
            2 => 2f,
            3 => 3.5f,
            4 => 5f,
            5 => 7.5f,
            _ => 0,
        };
    }
    public static int ClubDamageBuff(int weaponsLevel)
    {
        if (weaponsLevel > 0) return 8;
        return 0;
    }
    public static float ClubAttackSpeedDenominator(int weaponsLevel)
    {
        if (weaponsLevel > 0) return 1.1f;
        return 1f;
    }
    public static int SmallStoneDamageBuff(int weaponsLevel)
    {
        if (weaponsLevel > 1) return 6;
        return 0;
    }
    public static int AxeDamageBuff(int weaponsLevel)
    {
        if (weaponsLevel > 2) return 10;
        return 0;
    }
    public static float AxeAttackSpeedDenominator(int weaponsLevel)
    {
        if (weaponsLevel > 2) return 1.16f;
        return 1f;
    }
    public static int BowDamageBuff(int weaponsLevel)
    {
        if (weaponsLevel > 3) return 10;
        return 0;
    }
    public static float BowAttackSpeedDenominator(int weaponsLevel)
    {
        if (weaponsLevel > 3) return 1.12f;
        return 1f;
    }
    public static int StonesAndArrowsCapacityBuff(int efficiencyLevel)
    {
        if (efficiencyLevel > 0) return 2;
        else if (efficiencyLevel > 1) return 4;
        return 0;
    }
    public static int RewardBuff(int efficiencyLevel, int reward)
    {
        if (efficiencyLevel > 1) return Mathf.RoundToInt(reward * 1.1f);
        return 0;
    }
    public static int SpearCapacityBuff(int efficiencyLevel)
    {
        if (efficiencyLevel > 2) return 1;
        return 0;
    }
    public static int LeadershipHealthBuff(int leadershipLevel)
    {
        if (leadershipLevel > 2) return 30;
        else if (leadershipLevel > 0) return 10;
        return 0;
    }
    public static float LeadershipSpeedBuff(int leadershipLevel)
    {
        if (leadershipLevel > 1) return 0.3f;
        return 0f;
    }
    public static int LeadershipStoneCapacityBuff(int leadershipLevel)
    {
        if (leadershipLevel > 1) return 2;
        return 0;
    }
    public static float LeadershipAttackSpeedDenominator(int leadershipLevel)
    {
        if (leadershipLevel > 2) return 1.1f;
        return 1f;
    }
}
