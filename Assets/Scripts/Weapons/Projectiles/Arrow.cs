using UnityEngine;

public class Arrow : Projectile
{
    public GameObject hitSoundObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit || !directorComponent.activeSelf) return;

        base.HandleCollision(other.gameObject);
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.SlowDownEnemy();
            hitSoundObject.SetActive(true);
            hitSoundObject.transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
