using UnityEngine;

public class AttackModeCreatureSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnsParentObject;
    public Transform[] spawns;
    public ToninaWarrior[] friendlies;
    public Enemy[] enemies;

    public ObjectPooler pooler;

    void Start()
    {
        pooler = ObjectPooler.Instance;

        // Store all friendlies in an array
        friendlies = FindObjectsByType<ToninaWarrior>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        // Store all enemies (Chioh Clan) in an array
        enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        // Store all spawns in an array
        spawns = new Transform[spawnsParentObject.transform.childCount];
        for (int i = 0; i < spawnsParentObject.transform.childCount; i++)
        {
            spawns[i] = spawnsParentObject.transform.GetChild(i).transform;
        }
    }

    public void SetFriendliesActive(bool value)
    {
        foreach (ToninaWarrior warrior in friendlies)
        {
            warrior.gameObject.SetActive(value);
            if (value) warrior.ResetFriendlyAttributes();
        }
    }

    public void SetEnemiesActive(bool value)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(value);
            if (value) enemy.ResetEnemyAttributes();
        }
    }

    public void MoveFriendliesToSpawns(int[] spawnArray)
    {
        int creatureIndex = 0;

        for (int i = 0; i < spawns.Length; i++)
        {
            for (int j = 0; j < spawnArray[i]; j++)
            {
                if (creatureIndex >= friendlies.Length)
                {
                    Debug.LogWarning("Not enough creatures for the given spawns array!");
                    return;
                }

                friendlies[creatureIndex].transform.position = spawns[i].position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                creatureIndex++;
            }
        }
    }
}
