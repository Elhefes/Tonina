using UnityEngine;

public class SmallStone : Projectile
{
    public GameObject hitSoundObject;

    public override void Attack(Animator animator)
    {
        animator.SetTrigger("StoneAttack");
        SpawnProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If weapon switching is spammed, it's possible that canHit = true, while directorComponent isn't active.
        if (!canHit || !directorComponent.gameObject.activeSelf) return;

        HandleCollision(other.gameObject);
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.SlowDownEnemy();
            if (hitSoundObject != null)
            {
                hitSoundObject.SetActive(true);
                hitSoundObject.transform.SetParent(null);
            }
            Destroy();
        }
    }
}
