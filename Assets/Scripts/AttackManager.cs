using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public Player player;

    public void IniateDamage()
    {
        player.weaponOnHand.canHit = true;
    }

    public void FinishDamage()
    {
        player.weaponOnHand.canHit = false;
    }
}
