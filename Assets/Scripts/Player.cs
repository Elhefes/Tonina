using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;

    public WeaponType weaponOnHand;
    public GameObject[] weaponObjects;

    private void Start()
    {
        weaponOnHand = WeaponType.Club;
    }

    public void SwitchWeapon(WeaponType weaponType)
    {
        foreach (GameObject obj in weaponObjects)
        {
            obj.SetActive(false);
        }
        if (weaponType == WeaponType.Club) weaponObjects[0].SetActive(true);
        if (weaponType == WeaponType.Small_stone) weaponObjects[1].SetActive(true);
        if (weaponType == WeaponType.Spear) weaponObjects[2].SetActive(true);
    }
}
