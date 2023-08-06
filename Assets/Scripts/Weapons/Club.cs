using UnityEngine;

public class Club : Weapon
{
    public override void Attack(Animator animator)
    {
        animator.SetTrigger("ClubAttack");
    }
}
