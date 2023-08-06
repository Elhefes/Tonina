using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<WeaponType> availableWeapons;

    void Start()
    {
        availableWeapons = new List<WeaponType>();
        InitiateWeapons();
    }

    // implement the persistence loading of available weapons
    void InitiateWeapons()
    {
        availableWeapons.Add(WeaponType.Club);
        availableWeapons.Add(WeaponType.Small_stone);
        availableWeapons.Add(WeaponType.Spear);
    }
}
