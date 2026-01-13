using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    public GameObject friendlyWarrior;
    public GameObject enemyClubber;
    public GameObject enemyRunner;
    public GameObject enemySpearWarrior;
    public GameObject enemyAxeWarrior;

    public GameObject smallStone;
    public GameObject arrow;
    public GameObject spear;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    private void Awake()
    {
        Instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform.position, transform.rotation);
                obj.transform.parent = transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public void ResetPools()
    {
        // Clear all existing queues
        foreach (var key in poolDictionary.Keys)
        {
            poolDictionary[key].Clear();
        }

        // Go through all children of this ObjectPooler and reset them
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            obj.SetActive(false);

            // Try to find which pool this belongs to
            foreach (Pool pool in pools)
            {
                if (obj.name.Contains(pool.prefab.name)) // matching prefab type
                {
                    poolDictionary[pool.tag].Enqueue(obj);
                    break;
                }
            }
        }
    }

    public JadeaWarrior SpawnFriendlyFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
#if UNITY_EDITOR
            print("Pool with tag " + tag + " doesn't exist.");
#endif
            return null;
        }

        if (poolDictionary[tag].Count == 0) return SpawnNewFriendly(position, rotation);

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        return objectToSpawn.GetComponent<JadeaWarrior>();
    }

    public void AddFriendlyToPool(JadeaWarrior friendly, string tag)
    {
        poolDictionary[tag].Enqueue(friendly.gameObject);
        friendly.gameObject.SetActive(false);
        friendly.ResetFriendlyAttributes();
    }

    JadeaWarrior SpawnNewFriendly(Vector3 position, Quaternion rotation)
    {
        GameObject prefab = friendlyWarrior;
        JadeaWarrior newFriendly = Instantiate(prefab, position, rotation).GetComponent<JadeaWarrior>();
        newFriendly.SetFriendlyInPool(false);
        newFriendly.transform.parent = transform;
        return newFriendly;
    }

    public Enemy SpawnEnemyFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
#if UNITY_EDITOR
            print("Pool with tag " + tag + " doesn't exist.");
#endif
            return null;
        }

        if (poolDictionary[tag].Count == 0) return SpawnNewEnemy(tag, position, rotation);

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        return objectToSpawn.GetComponent<Enemy>();
    }

    public void AddEnemyToPool(Enemy enemy, string tag)
    {
        poolDictionary[tag].Enqueue(enemy.gameObject);
        enemy.gameObject.SetActive(false);
        enemy.ResetEnemyAttributes();
    }

    Enemy SpawnNewEnemy(string enemyType, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = enemyClubber;
        switch (enemyType)
        {
            case "Runner": prefab = enemyRunner; break;
            case "SpearWarrior": prefab = enemySpearWarrior; break;
            case "AxeWarrior": prefab = enemyAxeWarrior; break;
        }
        Enemy newEnemy = Instantiate(prefab, position, rotation).GetComponent<Enemy>();
        newEnemy.SetEnemyInPool(false);
        newEnemy.transform.parent = transform;
        return newEnemy;
    }

    public Projectile SpawnProjectileFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
#if UNITY_EDITOR
            print("Pool with tag " + tag + " doesn't exist.");
#endif
            return null;
        }

        if (poolDictionary[tag].Count == 0) return SpawnNewProjectile(tag, position, rotation);

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        return objectToSpawn.GetComponent<Projectile>();
    }

    public void AddProjectileToPool(Projectile projectile, string tag)
    {
        poolDictionary[tag].Enqueue(projectile.gameObject);
        projectile.gameObject.SetActive(false);
    }

    Projectile SpawnNewProjectile(string projectileType, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = smallStone;

        switch (projectileType)
        {
            case "Arrow": prefab = arrow; break;
            case "Spear": prefab = spear; break;
        }

        Projectile newProjectile = Instantiate(prefab, position, rotation).GetComponent<Projectile>();
        newProjectile.SetProjectileInPool(false);
        newProjectile.transform.parent = transform;
        return newProjectile;
    }
}