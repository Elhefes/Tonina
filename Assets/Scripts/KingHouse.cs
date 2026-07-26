using UnityEngine;

public class KingHouse : MonoBehaviour
{
    public Transform playerSpawnPosition;
    public Vector3 battlefieldStartingPosition;
    public Vector3 battlefieldAngle;
    public GameObject miniPyramid;
    public GameObject namePlate;
    public GameObject attributeUnlockScene;
    public GameObject namePlateTorchFlame1;
    public GameObject namePlateTorchFlame2;
    public GameObject[] plaques;
    private float[] plaqueXValues = { 8.75f, 7.75f, 6.75f, 5.75f, 4.75f };

    private void OnEnable()
    {
        UpdateNamePlate();
    }

    private void Start()
    {
        UpdateNamePlateTorchFlames();
        UpdatePlaquePositions();
    }

    public void UpdateNamePlate()
    {
        if (namePlate != null)
        {
            namePlate.name = "King " + PlayerProfile.playerName;
        }
    }

    public void UpdateNamePlateTorchFlames()
    {
        // Torches are lit if attribute points are available
        if (GameState.Instance.progressionData.availableAttributePoints > 0)
        {
            namePlateTorchFlame1.SetActive(true);
            namePlateTorchFlame2.SetActive(true);
        }
        else
        {
            namePlateTorchFlame1.SetActive(false);
            namePlateTorchFlame2.SetActive(false);
        }
    }

    public void UpdatePlaquePositions()
    {
        string customWeaponOrder = PlayerPrefs.GetString("CustomWeaponOrder", "01234");

        for (int i = 0; i < customWeaponOrder.Length; i++)
        {
            int plaqueIndex = customWeaponOrder[i] - '0'; // Convert string to int

            Transform plaque = plaques[plaqueIndex].transform;
            plaque.localPosition = new Vector3(
                plaqueXValues[i],
                plaque.localPosition.y,
                plaque.localPosition.z);
        }
    }
}
