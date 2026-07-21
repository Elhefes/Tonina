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

    private void OnEnable()
    {
        UpdateNamePlate();
    }

    private void Start()
    {
        UpdateNamePlateTorchFlames();
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
}
