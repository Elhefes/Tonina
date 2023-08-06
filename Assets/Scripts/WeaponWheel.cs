using UnityEngine;
using UnityEngine.UI;

public class WeaponWheel : MonoBehaviour
{
    public Button nextWeaponArrowButton;
    public Button nextWeaponButton;
    public Button previousWeaponArrowButton;
    public Button previousWeaponButton;

    public Animator weaponWheelAnimator;

    public Image currentWeaponImage;
    public Image nextWeaponImage;
    public Image previousWeaponImage;
    public Image[] weaponSprites;
    private int currentIndex;
    private int weaponIndex;
    private int slices = 5;
    public Weapon[] weapons;

    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }

    public void NextWeapon()
    {
        currentIndex++;
        weaponIndex++;
        if (currentIndex > slices - 1)
        {
            currentIndex = 0;
        }
        if (weaponIndex > weapons.Length - 1)
        {
            weaponIndex = 0;
        }
        var wep = weapons[weaponIndex];
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        weaponWheelAnimator.SetTrigger("NextWeapon");
    }

    public void PreviousWeapon()
    {
        currentIndex--;
        weaponIndex--;
        if (currentIndex < 0)
        {
            currentIndex = slices - 1;
        }
        if (weaponIndex < 0)
        {
            weaponIndex = weapons.Length - 1;
        }
        var wep = weapons[weaponIndex];
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        weaponWheelAnimator.SetTrigger("PreviousWeapon");
    }
}
