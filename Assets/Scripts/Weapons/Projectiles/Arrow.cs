using UnityEngine;

public class Arrow : Projectile
{
    public GameObject hitSoundObject;

    public override void UpdateWeaponValues()
    {
        if (recievesWeaponsBuffs)
        {
            damage = defaultDamage +
                PlayerAttributes.BowDamageBuff(GameState.Instance.progressionData.weaponsLevel);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit || !directorComponent.gameObject.activeSelf) return;

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
