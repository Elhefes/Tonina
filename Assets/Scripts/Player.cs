using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Player : Creature
{
    private int health;
    public int maxHealth;
    public int startingHealth;
    public Weapon[] weapons;
    private string weaponOrder;
    public Spear spear;
    public Bow bow;
    public SmallStone smallStone;
    public int spearStartingQuantity;
    public int arrowStartingQuantity;
    public int smallStoneStartingQuantity;
    public TMP_Text projectileQuantityTMP;
    public float defaultAttackStoppingDistance;
    public Slider healthBar;

    public PlayerHealthIndicator playerHealthIndicator;
    public OverHealBar overHealBar;
    public float secondsBeforeOverHealDecay;
    public float secondsBetweenOverHealDecayTicks;
    private bool overHealDecay;

    public bool insideKingHouse;
    public bool inVillage;

    public ClickToEnableObject clickToEnableObject;

    public OptionsMenu optionsMenu;

    public BattlefieldMenu battlefieldMenu;
    public LosingScreen losingScreen;
    public BarricadesController barricadeController;
    public GameObject villageTPMenu;

    public BuildingRemover buildingRemover;
    private PlaceableBuilding selectedPlaceableBuilding;
    public GameObject placeableBuildings; // Use this only for hiding e.g. when removing in build mode
    private bool readyToRemove;

    public MaizePlace maizePlace;
    public FillOkil fillOkil;
    private SpearRack spearRack;
    private BuildingRoof buildingRoof;
    private Villager villager;
    private GameObject currentTextSubject;
    private GameObject weatherStone;
    public GameObject weatherGame;
    public GameObject weatherGameResults;
    public int startingMaize;
    public int maxMaize;
    public int maizeHealAmount;
    public GameObject maizeInventory;
    public GameObject maizePickUp;
    public TMP_Text maizeAmountTMP;
    public TMP_Text maizeInPlaceTMP;
    private int maizeAmount;
    public GameObject fillOkilPickUp;
    public Image fillOkilButtonFill;
    public FillOkilHoldButton fillOkilHoldButton;
    public TMP_Text stonesInFillTMP;
    public TMP_Text spearsInFillTMP;
    public TMP_Text arrowsInFillTMP;

    public Button textBox;
    public TMP_Text speakerNameText;
    public TMP_Text textBoxText;
    private string[] linesToRead;
    public int textLineIndex;

    public Vector3 destination;
    public Vector3 kingHouseSpawnPosition;
    public GameObject blackFader;
    private Coroutine teleportCoroutine;

    private GameObject miniPyramid; // Used in main menu teleport

    public ClickerMaterial clickerObject;

    public Light weaponRangeIndicatorLight;

    private void Start()
    {
        health = startingHealth;
        maizeAmount = startingMaize;
        maizeAmountTMP.text = startingMaize.ToString();
        if (startingMaize > 0) maizeInventory.SetActive(true);
        SetProjectilesToMax();
    }

    public void EnableBattleMode()
    {
        weaponOrder = PlayerPrefs.GetString("CustomWeaponOrder", "0123");
        EquipDefaultWeapon();
        healthBar.gameObject.SetActive(true);
        SetProjectilesToMax();
        UpdateProjectileQuantityText();
    }

    public void DisableBattleMode()
    {
        foreach (Weapon obj in weapons)
        {
            obj.gameObject.SetActive(false);
        }
        if (health < startingHealth) RestoreHealth(startingHealth - health);
        healthBar.gameObject.SetActive(false);
        creatureMovement.animator.SetInteger("WeaponIndex", 0);
        battlefieldMenu.waveController.musicPlayer.PlayPeacefulSongs(false);
    }

    void EquipDefaultWeapon()
    {
        weapons[weaponOrder[0] - '0'].gameObject.SetActive(true);
        creatureMovement.animator.SetInteger("WeaponIndex", weaponOrder[0] - '0');
    }

    void SetProjectilesToMax()
    {
        // Idea: projectile quantities could be persistent and have to be refilled somewhere before a battle
        spear.quantity = spearStartingQuantity;
        spear.notAvailable = false;
        bow.quantity = arrowStartingQuantity;
        bow.notAvailable = false;
        smallStone.quantity = smallStoneStartingQuantity;
        smallStone.notAvailable = false;
    }

    private void Update()
    {
        base.Update();
        if (Input.GetKeyDown("m") && maizeAmount > 0) EatMaize();
        Debug.DrawLine(transform.position, transform.position + transform.forward * 114f, Color.red);

        if (creatureMovement.target != null) clickerObject.gameObject.transform.position = creatureMovement.target.transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // Check if there is a touch
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Check if finger is over a UI element
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
            }
            creatureMovement.target = null;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                GameObject target = hit.collider.gameObject;

                if (target.CompareTag("TalkPrompt") && Vector3.Distance(transform.position, target.transform.position) < 2.7f)
                {
                    if (target == currentTextSubject)
                    {
                        ReadNextLine();
                        return;
                    }
                    else
                    {
                        FreeTextSubject();
                        villager = target.GetComponent<Villager>();
                        if (villager != null)
                        {
                            SetTextLines(villager.textSubject.textLines);
                            villager.textSubject.textIsActive = true;
                            villager.TalkToPlayer(gameObject);
                            currentTextSubject = villager.gameObject;
                            villager.textSubject.currentIndex = 0;
                            villager.ProcessNextLines();
                        }
                        else
                        {
                            TextSubject textSubject = target.GetComponent<TextSubject>();
                            SetTextLines(textSubject.textLines);
                            currentTextSubject = textSubject.gameObject;
                            textSubject.currentIndex = 0;
                        }
                    }

                    textLineIndex = 0;
                    textBox.gameObject.SetActive(true);
                    UpdateTextBox();
                    creatureMovement.agent.SetDestination(target.transform.position);
                    creatureMovement.agent.stoppingDistance = 1.7f;
                    return;
                }
                else
                {
                    FreeTextSubject();
                }

                if (target.CompareTag("Enemy"))
                {
                    creatureMovement.agent.stoppingDistance = defaultAttackStoppingDistance;
                    //creatureMovement.enemy = target.GetComponent<Enemy>();
                    creatureMovement.target = target.transform;
                }
                else creatureMovement.agent.stoppingDistance = 0.1f;

                // Check if player clicks the god pillar inside the weather temple
                if (target.name == "chaac_stone" && Vector3.Distance(target.transform.position, transform.position) < 2.85f)
                {
                    weatherStone = target;
                    creatureMovement.agent.SetDestination(weatherStone.transform.position);
                    creatureMovement.agent.stoppingDistance = 2.85f;
                    if (!weatherGameResults.activeSelf) weatherGame.SetActive(true);
                    return;
                }
                else
                {
                    weatherGame.SetActive(false);
                    weatherStone = null;
                }

                if (target.CompareTag("VillageTPSpot") && Vector3.Distance(target.transform.position, transform.position) < 4f)
                {
                    creatureMovement.agent.SetDestination(Vector3.Lerp(transform.position, target.transform.position, 0.5f));
                    villageTPMenu.SetActive(true);
                    return;
                }
                else villageTPMenu.SetActive(false);

                // Click 1st object to enable the 2nd. Click 1st again to disable 2nd.
                if (target.CompareTag("ClickToEnableObject") && Vector3.Distance(target.transform.position, transform.position) < 4f)
                {
                    if (clickToEnableObject != null && clickToEnableObject.objectToEnable.activeSelf)
                    {
                        clickToEnableObject.objectToEnable.SetActive(false);
                        clickToEnableObject = null;
                        creatureMovement.agent.SetDestination(gameObject.transform.position);
                        return;
                    }
                    else
                    {
                        clickToEnableObject = target.GetComponent<ClickToEnableObject>();
                        clickToEnableObject.objectToEnable.SetActive(true);
                        creatureMovement.agent.stoppingDistance = 3f;
                    }
                }
                else
                {
                    if (clickToEnableObject != null)
                    {
                        clickToEnableObject.objectToEnable.SetActive(false);
                        clickToEnableObject = null;
                    }
                }

                if (readyToRemove)
                {
                    if (target.CompareTag("PlaceableBuilding"))
                    {
                        if (selectedPlaceableBuilding == null)
                        {
                            selectedPlaceableBuilding = target.GetComponentInParent<PlaceableBuilding>();
                            buildingRemover.SelectBuilding(selectedPlaceableBuilding);
                        }
                        else if (target.gameObject == selectedPlaceableBuilding.gameObject)
                        {
                            buildingRemover.SelectBuilding(selectedPlaceableBuilding);
                        }
                        else
                        {
                            selectedPlaceableBuilding = target.GetComponentInParent<PlaceableBuilding>();
                            buildingRemover.SelectBuilding(selectedPlaceableBuilding);
                        }
                        return;
                    }
                }

                if (battlefieldMenu.waveController.battleUI.activeSelf && (target.name == "spear_rack_small(Clone)") && Vector3.Distance(target.transform.position, transform.position) < 2.25f)
                {
                    if (target != null)
                    {
                        TryToTakeSpearFromRack(target);
                        return;
                    }
                }

                creatureMovement.agent.SetDestination(hit.point);
                destination = hit.point;
                if (creatureMovement.target != null) clickerObject.alpha = 1f;
            }
        }

        // Rotate towards text subject when it exists
        if (currentTextSubject != null) LookAt(currentTextSubject.gameObject.transform, true);

        // Rotate towards weather stone when it's clicked up close
        else if (weatherStone != null) LookAt(weatherStone.transform, true);

        else if (clickToEnableObject != null)
        {
            if (clickToEnableObject.objectToEnable.activeSelf) LookAt(clickToEnableObject.gameObject.transform, true);
        }

        if (weaponRangeIndicatorLight.intensity > 0)
        {
            weaponRangeIndicatorLight.intensity -= 0.007f;
        }
    }

    private void FixedUpdate()
    {
        if (fillOkilHoldButton.buttonPressed)
        {
            if (fillOkil.stoneAmount == 0 && fillOkil.spearAmount == 0 && fillOkil.arrowAmount == 0) return;
            fillOkilButtonFill.fillAmount += 1 / 150f;
            if (fillOkilButtonFill.fillAmount >= 1)
            {
                TakeAllFromFillOkil();
                fillOkilButtonFill.fillAmount = 0;
            }
        }
        else
        {
            if (fillOkilPickUp.activeSelf) fillOkilButtonFill.fillAmount = 0;
        }
    }

    void SetTextLines(string[] textLines)
    {
        linesToRead = textLines;
    }

    void UpdateTextBox()
    {
        speakerNameText.text = currentTextSubject.gameObject.name;
        if (textLineIndex < linesToRead.Length)
        {
            textBoxText.text = linesToRead[textLineIndex];
        }
        else
        {
            FreeTextSubject();
        }
    }

    public void ReadNextLine()
    {
        textLineIndex++;
        if (villager != null) villager.ProcessNextLines();
        UpdateTextBox();
    }

    public void FreeTextSubject()
    {
        if (villager != null)
        {
            if (villager.textSubject.UI_popUpAfterLastLine && textLineIndex == villager.textSubject.textLines.Length)
            {
                if (villager.textSubject.popUpElements != null)
                {
                    foreach (GameObject element in villager.textSubject.popUpElements) element.SetActive(true);
                }
            }

            villager.textSubject.textIsActive = false;
            villager.playingVoiceLines = false;
            villager.textSubject.currentIndex = 0;
            villager = null;
        }
        currentTextSubject = null;
        textBox.gameObject.SetActive(false);
    }

    public void SwitchWeapon(WeaponType weaponType)
    {
        if (this == null) return;
        foreach (Weapon obj in weapons)
        {
            obj.gameObject.SetActive(false);
        }

        int weaponIndex = 0;
        if (weaponType == WeaponType.Spear) weaponIndex = 1;
        if (weaponType == WeaponType.Axe) weaponIndex = 2;
        if (weaponType == WeaponType.Bow) weaponIndex = 3;
        if (weaponType == WeaponType.Small_stone) weaponIndex = 4;

        weaponOnHand = weapons[weaponIndex];
        weaponOnHand.canHit = false;
        weapons[weaponIndex].gameObject.SetActive(true);
        creatureMovement.animator.SetInteger("WeaponIndex", weaponIndex);
        UpdateWeaponRangeIndicator();
        UpdateProjectileQuantityText();
    }

    private void UpdateProjectileQuantityText()
    {
        if (spear.gameObject.activeSelf)
        {
            projectileQuantityTMP.text = spear.quantity.ToString();
            projectileQuantityTMP.gameObject.SetActive(true);
        }
        else if (smallStone.gameObject.activeSelf)
        {
            projectileQuantityTMP.text = smallStone.quantity.ToString();
            projectileQuantityTMP.gameObject.SetActive(true);
        }
        else if (bow.gameObject.activeSelf)
        {
            projectileQuantityTMP.text = bow.quantity.ToString();
            projectileQuantityTMP.gameObject.SetActive(true);
        }
        else projectileQuantityTMP.gameObject.SetActive(false);
    }

    private void UpdateWeaponRangeIndicator()
    {
        weaponRangeIndicatorLight.spotAngle = 2 * Mathf.Asin((weaponOnHand.attackDistance / 25f)) * 180 / Mathf.PI;
        weaponRangeIndicatorLight.intensity = 1.75f;
    }

    public void TakeDamage(int damage)
    {
        if (health <= startingHealth)
        {
            healthBar.value -= (float)damage / startingHealth;
        }
        health -= damage;
        if (health <= 0)
        {
            onCooldown = false;
            shouldAttack = false;

            health = 0;
            losingScreen.gameObject.SetActive(true);
            losingScreen.SetPlayerDied(true);
            gameObject.SetActive(false);
        }
        if (health + damage >= startingHealth) overHealBar.UpdateOverHealBar(health, startingHealth);
        playerHealthIndicator.UpdateHealthIndicator(health);
        print(health);
    }

    public void EatMaize()
    {
        // Eat 1 maize and remove it from maizeInventory
        if (health == maxHealth || maizeAmount < 1) return;
        RestoreHealth(maizeHealAmount);
        maizeAmount--;
        maizeAmountTMP.text = maizeAmount.ToString();
        if (maizeAmount < 1) maizeInventory.SetActive(false);
    }

    public void RestoreHealth(int amount)
    {
        healthBar.value += (float)amount / startingHealth;
        if (health + amount > maxHealth)
        {
            health += maxHealth - health;
        }
        else health += amount;
        if (health > startingHealth)
        {
            overHealBar.UpdateOverHealBar(health, startingHealth);
            if (!overHealDecay) StartCoroutine(OverHealDecay());
        }
        playerHealthIndicator.UpdateHealthIndicator(health);
        print(health);
    }

    private IEnumerator OverHealDecay()
    {
        overHealDecay = true;
        yield return new WaitForSeconds(secondsBeforeOverHealDecay);

        while (health > startingHealth)
        {
            TakeDamage(1);
            // Waiting not needed when going from startingHealth + 1 -> startingHealth
            if (health > startingHealth) yield return new WaitForSeconds(secondsBetweenOverHealDecayTicks);
        }
        overHealDecay = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Maize Place(Clone)")
        {
            maizePlace = other.GetComponentInParent<MaizePlace>();
            EnterMaizePlace();
        }
        if (other.gameObject.name == "Fill-Okil(Clone)")
        {
            fillOkil = other.GetComponentInParent<FillOkil>();
            EnterFillOkil();
        }
        if (other.CompareTag("Building"))
        {
            buildingRoof = other.GetComponentInParent<BuildingRoof>();
            buildingRoof.MakeRoofInvisible();
            if (other.ToString().Equals("kinghouse_floor_mesh (UnityEngine.MeshCollider)"))
            {
                inVillage = false;
                insideKingHouse = true;
            }
        }

        // Prevent UI overlapping
        if (optionsMenu.gameObject.activeSelf) return;

        else if (other.CompareTag("BattlefieldPrompt"))
        {
            battlefieldMenu.gameObject.SetActive(true);
        }
        else if (other.CompareTag("VillageTPSpot"))
        {
            villageTPMenu.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Maize Place(Clone)") ExitMaizePlace();
        if (other.gameObject.name == "Fill-Okil(Clone)") { fillOkilPickUp.SetActive(false); }
        if (other.CompareTag("Building"))
        {
            buildingRoof = other.GetComponentInParent<BuildingRoof>();
            buildingRoof.MakeRoofVisible();
            if (other.ToString().Equals("kinghouse_floor_mesh (UnityEngine.MeshCollider)")) insideKingHouse = false;
        }
        if (other.CompareTag("BattlefieldPrompt"))
        {
            battlefieldMenu.gameObject.SetActive(false);
        }
        if (other.CompareTag("VillageTPSpot"))
        {
            villageTPMenu.SetActive(false);
        }
    }

    public void EnterMaizePlace()
    {
        maizeInventory.SetActive(true);
        if (maizePlace.maizeInPlace < 1) return;
        maizePickUp.SetActive(true);
        maizeInPlaceTMP.text = maizePlace.maizeInPlace.ToString();
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
        if (maizePlace.maizeInPlace < 1) maizePickUp.SetActive(false);
    }
    public void EnterFillOkil()
    {
        fillOkilPickUp.SetActive(true);
        fillOkilButtonFill.fillAmount = 0f;
        stonesInFillTMP.text = fillOkil.stoneAmount.ToString();
        spearsInFillTMP.text = fillOkil.spearAmount.ToString();
        arrowsInFillTMP.text = fillOkil.arrowAmount.ToString();
    }

    public void TakeAllFromFillOkil()
    {
        if (fillOkil.spearAmount > 0)
        {
            if (spear.quantity < spearStartingQuantity)
            {
                spear.quantity++;
                spear.notAvailable = false;
                fillOkil.spearAmount--;
                spearsInFillTMP.text = fillOkil.spearAmount.ToString();
            }
        }
        if (fillOkil.stoneAmount > 0)
        {
            if (spear.quantity < spearStartingQuantity)
            {
                smallStone.quantity++;
                smallStone.notAvailable = false;
                fillOkil.stoneAmount--;
                stonesInFillTMP.text = fillOkil.stoneAmount.ToString();
                if (smallStone.gameObject.activeSelf) projectileQuantityTMP.text = smallStone.quantity.ToString();
            }
        }
        if (fillOkil.arrowAmount > 0)
        {
            if (bow.quantity < arrowStartingQuantity)
            {
                bow.quantity++;
                bow.notAvailable = false;
                fillOkil.arrowAmount--;
                arrowsInFillTMP.text = fillOkil.arrowAmount.ToString();
                if (bow.gameObject.activeSelf) projectileQuantityTMP.text = bow.quantity.ToString();
            }
        }
    }

    void TryToTakeSpearFromRack(GameObject rackObj)
    {
        if (spearRack == null || rackObj != spearRack)
        {
            spearRack = rackObj.GetComponent<SpearRack>();
        }
        if (spearRack != null && spear.quantity < spearStartingQuantity && spearRack.numOfSpearsInRack > 0)
        {
            spear.quantity++;
            spear.notAvailable = false;
            spearRack.TakeSpear();
        }
    }

    public void SetReadyToRemove(bool value) { readyToRemove = value; }

    public void LoseBattle() { battlefieldMenu.waveController.LoseBattle(); }

    public void EndBattle()
    {
        DestroyEnemies();
        DestroyFriendlyWarriors();
        DisableBattleMode();
        barricadeController.RestoreBarricades();
    }

    void DestroyEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) { Destroy(enemy); }
    }

    void DestroyFriendlyWarriors()
    {
        GameObject[] friendlyWarriors = GameObject.FindGameObjectsWithTag("ToninaTribe");
        foreach (GameObject friendly in friendlyWarriors)
        {
            if (friendly.ToString().Equals("Akuxa (UnityEngine.GameObject)")) continue;
            Destroy(friendly);
        }
    }

    public void ReturnHome(GameObject objectToDisable)
    {
        if (insideKingHouse) return;
        // Confirmation in battle before returning home
        if ((battlefieldMenu.waveController.battleUI.activeSelf || optionsMenu.returnFromBuilder) && !optionsMenu.confirmReturnHomeMenu.activeSelf)
        {
            optionsMenu.confirmReturnHomeMenu.SetActive(true);
            optionsMenu.normalMenu.SetActive(false);
            return;
        }
        if (objectToDisable != null) objectToDisable.SetActive(false);
        if (buildingRoof != null) buildingRoof.MakeRoofVisible();
        StartTeleportToHome();
    }

    public void StartTeleportToHome()
    {
        teleportCoroutine = StartCoroutine(TeleportPlayerToSpot(kingHouseSpawnPosition));
    }

    public void TeleportToVillageGate(Transform gatePosition)
    {
        if (villageTPMenu != null) villageTPMenu.SetActive(false);
        teleportCoroutine = StartCoroutine(TeleportPlayerToSpot(gatePosition.position));
        insideKingHouse = false;
        inVillage = true;
    }

    IEnumerator TeleportPlayerToSpot(Vector3 newPosition)
    {
        blackFader.SetActive(true);
        creatureMovement.agent.SetDestination(transform.position);
        yield return new WaitForSeconds(0.33f);
        if (miniPyramid != null)
        {
            if (Vector3.Distance(gameObject.transform.position, miniPyramid.transform.position) > 5f)
            {
                miniPyramid = null;
                yield break;
            }
        }
        miniPyramid = null;
        gameObject.SetActive(false);
        transform.position = newPosition;
        gameObject.SetActive(true);
    }

    public void FindMiniPyramid()
    {
        miniPyramid = GameObject.Find("tonina_pyramid_mini");
    }
}