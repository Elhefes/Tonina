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
            namePlate.name = "King " + GameState.Instance.GetPlayerName();
        }
    }
}
