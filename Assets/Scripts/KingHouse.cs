using UnityEngine;

public class KingHouse : MonoBehaviour
{
    public Transform playerSpawnPosition;
    public Vector3 battleFieldStartingPosition;
    public GameObject namePlate;

    private void OnEnable()
    {
        UpdateNamePlate();
    }

    public void UpdateNamePlate()
    {
        if (namePlate != null)
        {
            int selectedSave = PlayerPrefs.GetInt("selectedSaveFile", 1);
            if (selectedSave == 1) namePlate.name = "King " + PlayerPrefs.GetString("playerName1", "");
            else if (selectedSave == 2) namePlate.name = "King " + PlayerPrefs.GetString("playerName2", "");
            else if (selectedSave == 3) namePlate.name = "King " + PlayerPrefs.GetString("playerName3", "");
        }
    }
}
