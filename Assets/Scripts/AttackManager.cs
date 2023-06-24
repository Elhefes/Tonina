using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public Melee melee;

    public void IniateDamage()
    {
        melee.canHit = true;
    }

    public void FinishDamage()
    {
        melee.canHit = false;
    }
}
