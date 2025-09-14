using UnityEngine;

public class SmallStone : Projectile
{
    public GameObject hitSoundObject;

    public override void Attack(Animator animator)
    {
        animator.SetTrigger("StoneAttack");
        base.SpawnProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If weapon switching is spammed, it's possible that canHit = true, while directorComponent isn't active.
        if (!canHit || !base.directorComponent.activeSelf) return;

        base.HandleCollision(other.gameObject);
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.SlowDownEnemy();
            hitSoundObject.SetActive(true);
            hitSoundObject.transform.SetParent(null);
            Destroy();
        }
    }
}
