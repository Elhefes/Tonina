using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingWheel : MonoBehaviour
{
    public Button nextBuildingArrowButton;
    public Button nextBuildingButton;
    public Button previousBuildingArrowButton;
    public Button previousBuildingButton;

    public Animator buildingWheelAnimator;

    private bool buildingWheelCooldown;
    public float coolDownTime;

    public TMP_Text buildingsPlacedText;
    public GameObject noMoreRoomForBuildingsIndicator;

    public Image currentBuildingImage;
    public Image nextBuildingImage;
    public Image previousBuildingImage;
    public Image[] buildingSprites;
    private int currentIndex;
    private int buildingIndex;
    private int slices = 5;
    public BuildingsManager buildingsManager;
    public BuildingPlacing[] placeableBuildingsOnPlayer;
    private List<GameObject> buildingsToBePlaced = new List<GameObject>();
    private int buildingCountAtModeStart;

    public AudioSource soundEffectPlayer;

    public Player player;

    private void OnEnable()
    {
        buildingCountAtModeStart = buildingsManager.buildingsPlaced;
        UpdateBuildingsPlacedText();
        ResetToDefaultBuilding();
        ShowBuildingInHandIfPossible();
    }

    public void ExitBuildMode(bool saving)
    {
        foreach (BuildingPlacing building in placeableBuildingsOnPlayer)
        {
            building.gameObject.SetActive(false);
        }
        if (!saving)
        {
            foreach (GameObject obj in buildingsToBePlaced)
            {
                Destroy(obj);
            }
            buildingsManager.buildingsPlaced = buildingCountAtModeStart;
        }
        buildingsToBePlaced.Clear();
        player.StartTeleportToHome();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) NextBuilding();
        if (Input.GetKeyDown(KeyCode.Q)) PreviousBuilding();
    }

    void UpdateBuildingsPlacedText()
    {
        buildingsPlacedText.text = "Buildings placed:\n" + buildingsManager.buildingsPlaced + " / " + buildingsManager.maxBuildingAmount;
        if (buildingsManager.buildingsPlaced < buildingsManager.maxBuildingAmount)
        {
            noMoreRoomForBuildingsIndicator.SetActive(false);
        }
        else
        {
            noMoreRoomForBuildingsIndicator.SetActive(true);
        }
    }

    public void ShowBuildingInHandIfPossible()
    {
        if (buildingsManager.buildingsPlaced < buildingsManager.maxBuildingAmount)
        {
            player.placeableBuildings.SetActive(true);
        }
        else
        {
            player.placeableBuildings.SetActive(false);
        }
    }

    public void NextBuilding()
    {
        if (buildingWheelCooldown) return;
        StartBuildingWheelCooldown();
        currentIndex++;
        buildingIndex++;
        if (currentIndex > slices - 1)
        {
            currentIndex = 0;
        }
        if (buildingIndex > placeableBuildingsOnPlayer.Length - 1)
        {
            buildingIndex = 0;
        }
        var building = placeableBuildingsOnPlayer[buildingIndex];
        buildingSprites[currentIndex].sprite = building.uiSprite;
        SwitchToBuilding(buildingIndex);
        buildingWheelAnimator.SetTrigger("NextInWheel");
        // Add switch sound later?
        //if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }

    public void PreviousBuilding()
    {
        if (buildingWheelCooldown) return;
        StartBuildingWheelCooldown();
        currentIndex--;
        buildingIndex--;
        if (currentIndex < 0)
        {
            currentIndex = slices - 1;
        }
        if (buildingIndex < 0)
        {
            buildingIndex = placeableBuildingsOnPlayer.Length - 1;
        }
        var building = placeableBuildingsOnPlayer[buildingIndex];
        buildingSprites[currentIndex].sprite = building.uiSprite;
        SwitchToBuilding(buildingIndex);
        buildingWheelAnimator.SetTrigger("PreviousInWheel");
        // Add switch sound later?
        //if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }

    void SwitchToBuilding(int buildingIndex)
    {
        if (this == null) return;
        foreach (BuildingPlacing building in placeableBuildingsOnPlayer)
        {
            building.gameObject.SetActive(false);
        }
        placeableBuildingsOnPlayer[buildingIndex].gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f); // Reset rotation
        placeableBuildingsOnPlayer[buildingIndex].gameObject.SetActive(true);
    }

    public void TryToPlaceBuilding()
    {
        if (buildingsManager.buildingsPlaced >= buildingsManager.maxBuildingAmount)
        {
            return;
        }
        foreach (BuildingPlacing building in placeableBuildingsOnPlayer) // This could be optimised by only recieving the one that's active?
        {
            if (building.gameObject.activeSelf)
            {
                if (building.canPlace)
                {
                    GameObject obj = Instantiate(building.prefabObject, building.gameObject.transform.position, building.gameObject.transform.rotation);
                    buildingsToBePlaced.Add(obj);
                    buildingsManager.buildingsPlaced++;
                    UpdateBuildingsPlacedText();
                    ShowBuildingInHandIfPossible();
                }
            }
        }
    }

    public void RemoveBuildingsByAmount(int amount)
    {
        buildingsManager.buildingsPlaced = buildingsManager.buildingsPlaced - amount;
        UpdateBuildingsPlacedText();
    }

    public void RotatePlaceableBuilding(int degrees)
    {
        placeableBuildingsOnPlayer[buildingIndex].gameObject.transform.eulerAngles += new Vector3(0f, degrees, 0f);
    }

    void StartBuildingWheelCooldown()
    {
        Invoke("ResetBuildingWheelCooldown", coolDownTime);
        buildingWheelCooldown = true;
    }

    void ResetBuildingWheelCooldown()
    {
        buildingWheelCooldown = false;
    }

    public void ResetToDefaultBuilding()
    {
        var building = placeableBuildingsOnPlayer[0];
        currentIndex = 0;
        buildingIndex = 0;
        buildingSprites[currentIndex].sprite = building.uiSprite;
        SwitchToBuilding(buildingIndex);
        // Reset rotation of building wheel's circle
        buildingWheelAnimator.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
