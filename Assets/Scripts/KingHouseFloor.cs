using UnityEngine;

public class KingHouseFloor : MonoBehaviour
{
    public GameplayCameraAngles camAngles;
    public int kingHouseCamAngleInt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camAngles.SetCameraAngle(kingHouseCamAngleInt);
        }
    }
}
