using UnityEngine;

public class AttackModePositionButton : MonoBehaviour
{
    [SerializeField] private Vector3 positionToMoveTo;
    public MouseLook mouseLook;

    public void StartMovingToPosition()
    {
        mouseLook.StartMovingToPosition(positionToMoveTo);
    }
}
