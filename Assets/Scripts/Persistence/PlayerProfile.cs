using UnityEngine;

public static class PlayerProfile
{
    private const string defaultName = "Sartom";

    public static string playerName
    {
        get
        {
            int save = PlayerPrefs.GetInt("selectedSaveFile", 1);

            string name = save switch
            {
                1 => PlayerPrefs.GetString("playerName1", ""),
                2 => PlayerPrefs.GetString("playerName2", ""),
                3 => PlayerPrefs.GetString("playerName3", ""),
                _ => ""
            };

            return string.IsNullOrEmpty(name) ? defaultName : name;
        }
    }
}
