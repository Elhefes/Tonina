using UnityEngine;

public class BuildingPlacing : MonoBehaviour
{
    public Material placingMat;
    Color canPlaceColor = new Color(58f / 255f, 189f / 255f, 105f / 255f, 99f / 255f); // Green jade color
    Color cannotPlaceColor = new Color(255f / 255f, 0f / 255f, 0f / 255f, 99f / 255f);  // Red color

    public Transform bottomTransform;
    public float collisionRadius; // Change this later when there's more building shapes
    public LayerMask collisionLayerMask; // Which layer objects prevent building placement

    void Start()
    {
        placingMat.color = canPlaceColor;
    }

    private void Update()
    {
        if (Physics.OverlapSphere(bottomTransform.position, collisionRadius, LayerMask.GetMask("GroundForBuilding")).Length > 0)
        {
            if (Physics.OverlapSphere(bottomTransform.position, collisionRadius, collisionLayerMask).Length > 0)
            {
                placingMat.color = cannotPlaceColor;
                return;
            }
            else
            {
                placingMat.color = canPlaceColor;
            }
        }
        else
        {
            placingMat.color = cannotPlaceColor;
        }
    }
}
