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
    public GameObject[] weaponObjects;
    public float defaultAttackStoppingDistance;
    public Slider healthBar;

    public PlayerHealthIndicator playerHealthIndicator;
    public OverHealBar overHealBar;
    public float secondsBeforeOverHealDecay;
    public float secondsBetweenOverHealDecayTicks;
    private bool overHealDecay;

    public bool insideKingHouse;
    public bool inVillage;

    public BattlefieldMenu battlefieldMenu;
    public LosingScreen losingScreen;
    public GameObject villageTPMenu;

    public MaizePlace maizePlace;
    public FillOkil fillOkil;
    private BuildingRoof buildingRoof;
    private Villager villager;
    private GameObject currentTalkingSubject;
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
    public TMP_Text textBoxText;
    private string[] linesToRead;
    public int textLineIndex;

    public Vector3 destination;
    public Vector3 kingHouseSpawnPosition;
    public GameObject blackFader;
    private Coroutine teleportCoroutine;

    public ClickerMaterial clickerObject;

    public Light weaponRangeIndicatorLight;

    private float mapLimitZ = -140f;

    private void Awake()
    {
        health = startingHealth;
        maizeAmount = startingMaize;
        maizeAmountTMP.text = startingMaize.ToString();
        if (startingMaize > 0) maizeInventory.SetActive(true);
        // This could be moved to start when player enters battlefield
        InvokeRepeating("LimitMovementInZAxis", 2.5f, 2.5f);
    }

    public void EnableBattleMode()
    {
        EquipDefaultWeapon();
        healthBar.gameObject.SetActive(true);
    }

    public void DisableBattleMode()
    {
        foreach (GameObject obj in weaponObjects)
        {
            obj.SetActive(false);
        }
        if (health < startingHealth) RestoreHealth(startingHealth - health);
        healthBar.gameObject.SetActive(false);
        creatureMovement.animator.SetInteger("WeaponIndex", 0);
        battlefieldMenu.waveController.musicPlayer.PlayPeacefulSongs(false);
    }

    void EquipDefaultWeapon()
    {
        weaponObjects[0].SetActive(true);
        creatureMovement.animator.SetInteger("WeaponIndex", 0);
    }

    private void Update()
    {
        base.Update();
        if (Input.GetKeyDown("m") && maizeAmount > 0) EatMaize();
        Debug.DrawLine(transform.position, transform.position + transform.forward * 114f, Color.red);

        if (creatureMovement.target != null) clickerObject.gameObject.transform.position = creatureMovement.target.transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() || transform.position.z < mapLimitZ) return;

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
                    if (target != currentTalkingSubject) FreeVillagerFromTalking();
                    villager = target.GetComponent<Villager>();
                    //read next line if clicked on same villager
                    if (villager.talking)
                    {
                        ReadNextLine();
                        return;
                    }

                    textBox.gameObject.SetActive(true);
                    SetTextLines(villager);
                    villager.talking = true;
                    villager.TalkToPlayer(gameObject);
                    currentTalkingSubject = villager.gameObject;
                    textLineIndex = 0;
                    villager.currentIndex = 0;
                    villager.ProcessNextLines();
                    UpdateTextBox();
                    creatureMovement.agent.SetDestination(target.transform.position);
                    LookAt(target.transform);
                    creatureMovement.agent.stoppingDistance = 1.6f;
                    return;
                }
                else
                {
                    FreeVillagerFromTalking();
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
                    creatureMovement.agent.stoppingDistance = 2.5f;
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

                creatureMovement.agent.SetDestination(hit.point);
                destination = hit.point;
                if (creatureMovement.target != null) clickerObject.alpha = 1f;
            }
        }

        // Rotate towards villager when talking up close
        if (villager != null && villager.talking) LookAt(villager.gameObject.transform);

        // Rotate towards weather stone when it's clicked up close
        if (weatherStone != null) LookAt(weatherStone.transform);

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
            fillOkilButtonFill.fillAmount += 1 / 300f;
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

    void SetTextLines(Villager villager)
    {
        linesToRead = villager.textLines;
    }

    void UpdateTextBox()
    {
        if (textLineIndex < linesToRead.Length)
        {
            textBoxText.text = linesToRead[textLineIndex];
        }
        else
        {
            FreeVillagerFromTalking();
        }
    }

    public void ReadNextLine()
    {
        textLineIndex++;
        villager.ProcessNextLines();
        UpdateTextBox();
    }

    public void FreeVillagerFromTalking()
    {
        if (villager != null)
        {
            villager.talking = false;
            villager.playingVoiceLines = false;
            villager.currentIndex = 0;
            villager = null;
            currentTalkingSubject = null;
            textBox.gameObject.SetActive(false);
        }
    }

    public void SwitchWeapon(WeaponType weaponType)
    {
        if (this == null) return;
        foreach (GameObject obj in weaponObjects)
        {
            obj.SetActive(false);
        }

        int weaponIndex = 0;
        if (weaponType == WeaponType.Spear) weaponIndex = 1;
        if (weaponType == WeaponType.Axe) weaponIndex = 2;
        if (weaponType == WeaponType.Small_stone) weaponIndex = 3;

        weaponOnHand = weaponObjects[weaponIndex].GetComponent<Weapon>();
        weaponObjects[weaponIndex].SetActive(true);
        creatureMovement.animator.SetInteger("WeaponIndex", weaponIndex);
        weaponOnHand.canHit = false;
        UpdateWeaponRangeIndicator();
    }

    private void UpdateWeaponRangeIndicator()
    {
        weaponRangeIndicatorLight.spotAngle = Mathf.Asin((weaponOnHand.attackDistance / 25f)) * 180 / Mathf.PI;
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
        if (other.CompareTag("MaizePlace"))
        {
            maizePlace = other.GetComponentInParent<MaizePlace>();
            EnterMaizePlace();
        }
        if (other.CompareTag("Fill-Okil"))
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
        if (other.CompareTag("BattlefieldPrompt"))
        {
            battlefieldMenu.gameObject.SetActive(true);
        }
        if (other.CompareTag("VillageTPSpot"))
        {
            villageTPMenu.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MaizePlace"))
        {
            ExitMaizePlace();
        }
        if (other.CompareTag("Fill-Okil")) { fillOkilPickUp.SetActive(false); }
        if (other.CompareTag("Building"))
        {
            buildingRoof = other.GetComponentInParent<BuildingRoof>();
            buildingRoof.MakeRoofVisible();
            if (other.ToString().Equals("kinghouse_floor_mesh (UnityEngine.MeshCollider)")) insideKingHouse = false;
        }
        if (other.CompareTag("BattlefieldPrompt"))
        {
            battlefieldMenu.Exit();
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
        // When inventory space is added, this should give all of these at once until max. amount reached
        if (fillOkil.stoneAmount > 0)
        {
            fillOkil.stoneAmount--;
            stonesInFillTMP.text = fillOkil.stoneAmount.ToString();
        }
        if (fillOkil.spearAmount > 0)
        {
            fillOkil.spearAmount--;
            spearsInFillTMP.text = fillOkil.spearAmount.ToString();
        }
        if (fillOkil.arrowAmount > 0)
        {
            fillOkil.arrowAmount--;
            arrowsInFillTMP.text = fillOkil.arrowAmount.ToString();
        }
    }

    void LimitMovementInZAxis()
    {
        if (transform.position.z < mapLimitZ)
        {
            creatureMovement.agent.SetDestination(new Vector3(transform.position.x, transform.position.y, mapLimitZ));
        }
    }

    public void LoseBattle() { battlefieldMenu.waveController.LoseBattle(); }

    public void EndBattle()
    {
        DestroyEnemies();
        DestroyFriendlyWarriors();
        DisableBattleMode();
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
        if (objectToDisable != null) objectToDisable.SetActive(false);
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
        gameObject.SetActive(false);
        transform.position = newPosition;
        gameObject.SetActive(true);
    }
}