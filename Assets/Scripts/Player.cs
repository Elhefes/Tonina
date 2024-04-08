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

    public MaizePlace maizePlace;
    private BuildingRoof buildingRoof;
    private Villager villager;
    private GameObject currentTalkingSubject;
    public int startingMaize;
    public int maxMaize;
    public int maizeHealAmount;
    public GameObject maizeInventory;
    public GameObject maizePickUp;
    public TMP_Text maizeAmountTMP;
    public TMP_Text maizeInPlaceTMP;
    private int maizeAmount;

    public Button textBox;
    public TMP_Text textBoxText;
    private string[] linesToRead;
    public int textLineIndex;

    public Vector3 destination;

    public Light weaponRangeIndicatorLight;

    private void Awake()
    {
        health = startingHealth;
        maizeAmount = startingMaize;
        maizeAmountTMP.text = startingMaize.ToString();
        if (startingMaize > 0) maizeInventory.SetActive(true);
    }

    private void Update()
    {
        base.Update();
        if (Input.GetKeyDown("m") && maizeAmount > 0) EatMaize();
        Debug.DrawLine(transform.position, transform.position + transform.forward * 114f, Color.red);

        

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

                if (target.CompareTag("ClickBlocker") || target.CompareTag("ClickBlockerZ"))
                {
                    return;
                }

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
                    textBox.gameObject.SetActive(false);
                    FreeVillagerFromTalking();
                }

                if (target.CompareTag("Enemy"))
                {
                    creatureMovement.agent.stoppingDistance = defaultAttackStoppingDistance;
                    //creatureMovement.enemy = target.GetComponent<Enemy>();
                    creatureMovement.target = target.transform;
                }
                else creatureMovement.agent.stoppingDistance = 0.1f;

                creatureMovement.agent.SetDestination(hit.point);
                destination = hit.point;
            }
        }

        //rotate towards villager when talking up close
        if (villager != null && villager.talking) LookAt(villager.gameObject.transform);

        if (weaponRangeIndicatorLight.intensity > 0)
        {
            weaponRangeIndicatorLight.intensity -= 0.007f;
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
            textBox.gameObject.SetActive(false);
            FreeVillagerFromTalking();
        }
    }

    public void ReadNextLine()
    {
        textLineIndex++;
        villager.ProcessNextLines();
        UpdateTextBox();
    }

    void FreeVillagerFromTalking()
    {
        if (villager != null)
        {
            villager.talking = false;
            villager.playingVoiceLines = false;
            villager.currentIndex = 0;
            villager = null;
            currentTalkingSubject = null;
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
        if (weaponType == WeaponType.Axe) weaponIndex = 1;
        if (weaponType == WeaponType.Small_stone) weaponIndex = 2;
        if (weaponType == WeaponType.Spear) weaponIndex = 3;

        weaponOnHand = weaponObjects[weaponIndex].GetComponent<Weapon>();
        weaponObjects[weaponIndex].SetActive(true);
        creatureMovement.animator.SetInteger("WeaponIndex", weaponIndex);
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
            Destroy(gameObject);
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
        if (other.CompareTag("Building"))
        {
            buildingRoof = other.GetComponentInParent<BuildingRoof>();
            buildingRoof.MakeRoofInvisible();
            if (other.ToString().Equals("kinghouse_floor_mesh (UnityEngine.MeshCollider)")) insideKingHouse = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MaizePlace"))
        {
            ExitMaizePlace();
        }
        if (other.CompareTag("Building"))
        {
            buildingRoof = other.GetComponentInParent<BuildingRoof>();
            buildingRoof.MakeRoofVisible();
            if (other.ToString().Equals("kinghouse_floor_mesh (UnityEngine.MeshCollider)")) insideKingHouse = false;
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
}