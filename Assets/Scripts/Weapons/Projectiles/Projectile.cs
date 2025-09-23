using UnityEngine;
using TMPro;

public class Projectile : Weapon
{
    public Transform shootingPoint;
    public ProjectileDirectorComponent directorComponent;
    public int quantity;
    public bool infinite;
    public TMP_Text weaponWheelQuantityTMP;

    public ObjectPooler pooler;
    protected bool projectileInPool = true;

    private void Start()
    {
        pooler = ObjectPooler.Instance;
    }

    public void SpawnProjectile()
    {
        Projectile spawnedProjectile;
        if (pooler != null)
        {
            spawnedProjectile = pooler.SpawnProjectileFromPool(gameObject.name, shootingPoint.position, transform.rotation);
        }
        else
        {
            spawnedProjectile = Instantiate(this, shootingPoint.position, shootingPoint.rotation);
        }
        spawnedProjectile.shootingPoint = shootingPoint;
        spawnedProjectile.transform.position = shootingPoint.position;
        spawnedProjectile.directorComponent.creatureMovement = directorComponent.creatureMovement;
        spawnedProjectile.directorComponent.pointA = shootingPoint;
        spawnedProjectile.isFriendly = isFriendly;
        spawnedProjectile.directorComponent.rb.constraints = RigidbodyConstraints.None;

        spawnedProjectile.canHit = true;
        spawnedProjectile.directorComponent.gameObject.SetActive(true);

        if (!infinite) quantity--;
        if (quantity < 1) notAvailable = true;
        if (weaponWheelQuantityTMP != null) weaponWheelQuantityTMP.text = quantity.ToString();

        gameObject.SetActive(false);
    }

    public void SetProjectileInPool(bool value)
    {
        projectileInPool = value;
    }

    public void Destroy()
    {
        CancelInvoke();
        if (!gameObject.activeSelf) return;
        if (pooler != null && projectileInPool) ObjectPooler.Instance.AddProjectileToPool(this, gameObject.name.Substring(0, gameObject.name.Length - 7));
        else Destroy(gameObject);
    }
}
