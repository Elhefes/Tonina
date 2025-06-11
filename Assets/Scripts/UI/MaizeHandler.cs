using UnityEngine;
using TMPro;

public class MaizeHandler : MonoBehaviour
{
    public MaizePlace maizePlace;

    public DragMaizeIcon dragMaizeIcon;

    private Player player;

    public int startingMaize;
    public int maxMaize;
    public int maizeHealAmount;

    public GameObject maizeInventory;
    public GameObject maizeInventoryIcon;
    public GameObject maizePickUp;
    public GameObject maizePickUpIcon;
    public GameObject arrow;
    public TMP_Text maizeAmountTMP;
    public TMP_Text maizeInPlaceTMP;
    private int maizeAmount;

    private void Start()
    {
        player = GetComponent<Player>();
        maizeAmount = startingMaize;
        maizeAmountTMP.text = startingMaize.ToString();
        if (startingMaize > 0) maizeInventory.SetActive(true);
    }

    public void EnterMaizePlace()
    {
        maizeInventory.SetActive(true);
        if (maizePlace.maizeInPlace < 1) return;
        maizePickUp.SetActive(true);
        arrow.SetActive(true);
        maizeInPlaceTMP.text = maizePlace.maizeInPlace.ToString();
        UpdatePickupTransformPosition();
        UpdatePickupStartingPosition();
    }

    public void ExitMaizePlace()
    {
        if (maizeAmount < 1) maizeInventory.SetActive(false);
        maizePickUp.SetActive(false);
    }

    public void PickupMaize()
    {
        // Moves 1 maize from MaizePlace to player's inventory
        if (maizeAmount >= maxMaize) return;
        maizePlace.GetMaizeFromPlace();
        maizeInPlaceTMP.text = maizePlace.maizeInPlace.ToString();
        maizeInventory.SetActive(true);
        maizeAmount += 1;
        maizeAmountTMP.text = maizeAmount.ToString();
        if (maizePlace.maizeInPlace < 1)
        {
            maizePickUp.SetActive(false);
            return;
        }
        UpdatePickupTransformPosition();
        UpdatePickupStartingPosition();
    }

    private void UpdatePickupTransformPosition()
    {
        dragMaizeIcon.transform.position = clampedPos();
    }

    public void UpdatePickupStartingPosition()
    {
        dragMaizeIcon.mStartingPosition = clampedPos() + new Vector3(-960, -540, 0);
    }

    private Vector3 clampedPos()
    {
        // Starting position = Maize Place's center position on the screen, with restrictions
        Vector3 maizePos = Camera.main.WorldToScreenPoint(maizePlace.transform.position);
        return new Vector3(Mathf.Clamp(maizePos.x, 500, 1500), Mathf.Clamp(maizePos.y, 250, 830));
    }

    public void EatMaize()
    {
        // Eat 1 maize and remove it from maizeInventory
        if (player.health == player.maxHealth || maizeAmount < 1) return;
        player.RestoreHealth(maizeHealAmount);
        maizeAmount--;
        maizeAmountTMP.text = maizeAmount.ToString();
        if (maizeAmount < 1 && !maizePickUp.activeSelf) maizeInventory.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown("m") && maizeAmount > 0) EatMaize();
        UpdateArrow();
    }

    void UpdateArrow()
    {
        // 1. Calculate world positions of image centers
        Vector3 pos1 = dragMaizeIcon.transform.position;
        Vector3 pos2 = maizeInventoryIcon.transform.position;

        // 2. Place arrow at 70% from pos1 to pos2
        Vector3 targetPos = Vector3.Lerp(pos1, pos2, 0.7f);
        arrow.transform.position = targetPos;

        // 3. Compute direction vector from image1 to image2
        Vector3 direction = (pos2 - pos1).normalized;

        // 4. Calculate angle (in degrees) for arrow to point from image1 to image2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 5. Adjust because arrow's default is pointing "up" (along +Y)
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!player.buildModeUI.activeSelf) // If not in build mode
        {
            if (other.gameObject.name == "Maize Place(Clone)")
            {
                maizePlace = other.GetComponentInParent<MaizePlace>();
                EnterMaizePlace();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Maize Place(Clone)") ExitMaizePlace();
    }
}
