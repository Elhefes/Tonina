using UnityEngine;
using TMPro;

public class MaizeHandler : MonoBehaviour
{
    public MaizePlace maizePlace;

    private Player player;

    public int startingMaize;
    public int maxMaize;
    public int maizeHealAmount;

    public GameObject maizeInventory;
    public GameObject maizePickUp;
    public GameObject maizePickUpIcon;
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
        maizeInPlaceTMP.text = maizePlace.maizeInPlace.ToString();
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
        UpdatePickupStartingPosition();
    }

    void UpdatePickupStartingPosition()
    {
        // Starting position = Maize Place's center position on the screen, with restrictions
        Vector3 maizePos = Camera.main.WorldToScreenPoint(maizePlace.transform.position);
        maizePickUpIcon.transform.position = new Vector3(Mathf.Clamp(maizePos.x, 500, 1500), Mathf.Clamp(maizePos.y, 250, 830), 0);
    }

    public void EatMaize()
    {
        // Eat 1 maize and remove it from maizeInventory
        if (player.health == player.maxHealth || maizeAmount < 1) return;
        player.RestoreHealth(maizeHealAmount);
        maizeAmount--;
        maizeAmountTMP.text = maizeAmount.ToString();
        if (maizeAmount < 1) maizeInventory.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown("m") && maizeAmount > 0) EatMaize();
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
