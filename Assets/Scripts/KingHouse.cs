using UnityEngine;

public class KingHouse : MonoBehaviour
{
    public Transform playerSpawnPosition;
    public Vector3 battlefieldStartingPosition;
    public Vector3 battlefieldAngle;
    public GameObject miniPyramid;
    public GameObject namePlate;

    private void OnEnable()
    {
        UpdateNamePlate();
    }

    public void UpdateNamePlate()
    {
        if (namePlate != null)
        {
            namePlate.name = "King " + PlayerProfile.playerName;
        }
    }
}
