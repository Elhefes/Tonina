using UnityEngine;

public class BuildingPlacing : MonoBehaviour
{
    public Placeable placeableBuildingPrefab;
    public Material placingMat;
    public bool canPlace;
    public bool canPlaceOnGrass;
    Color canPlaceColor = new Color(58f / 255f, 189f / 255f, 105f / 255f, 99f / 255f); // Green jade color
    Color cannotPlaceColor = new Color(255f / 255f, 0f / 255f, 0f / 255f, 99f / 255f);  // Red color

    public Vector3 halfExtentsVector; // Input x = half of width, y = small number above 0, and z = half of depth
    public LayerMask collisionLayerMask; // Which layer objects prevent building placement

    public Sprite uiSprite;

    void Start()
    {
        placingMat.color = canPlaceColor;
    }

    private void Update()
    {
        if (isPlaceable())
        {
            placingMat.color = canPlaceColor;
            canPlace = true;
        }
        else
        {
            placingMat.color = cannotPlaceColor;
            canPlace = false;
        }
    }

    private bool isPlaceable()
    {
        string[] groundLayers = canPlaceOnGrass
            ? new[] { "GroundForBuilding", "Terrain" }
            : new[] { "GroundForBuilding" };

        bool isOnValidGround = Physics.OverlapBox(
            transform.position,
            halfExtentsVector,
            Quaternion.identity,
            LayerMask.GetMask(groundLayers)
        ).Length > 0;

        if (!isOnValidGround) return false;

        bool isColliding = Physics.OverlapBox(
            transform.position,
            halfExtentsVector,
            transform.rotation,
            collisionLayerMask
        ).Length > 0;

        return !isColliding;
    }
}
