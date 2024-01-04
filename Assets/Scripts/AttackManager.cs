using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public Creature creature;

    public void InitiateDamage()
    {
        creature.weaponOnHand.canHit = true;
    }

    public void FinishDamage()
    {
        creature.weaponOnHand.canHit = false;
    }
}
