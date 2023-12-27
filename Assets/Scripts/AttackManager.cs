using UnityEngine;

public class AttackManager : Creature
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
