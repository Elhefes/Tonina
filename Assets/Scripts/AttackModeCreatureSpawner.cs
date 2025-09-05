using UnityEngine;

public class AttackModeCreatureSpawner : MonoBehaviour
{
    public GameObject friendliesParentObject;
    [SerializeField] private GameObject spawnsParentObject;
    private GameObject[] friendlies;
    private Transform[] spawns;

    void Start()
    {
        // Store all friendlies in an array
        friendlies = new GameObject[friendliesParentObject.transform.childCount];
        for (int i = 0; i < friendliesParentObject.transform.childCount; i++)
        {
            friendlies[i] = friendliesParentObject.transform.GetChild(i).gameObject;
        }

        // Store all spawns in an array
        spawns = new Transform[spawnsParentObject.transform.childCount];
        for (int i = 0; i < spawnsParentObject.transform.childCount; i++)
        {
            spawns[i] = spawnsParentObject.transform.GetChild(i).transform;
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

                friendlies[creatureIndex].transform.position = spawns[i].position;
                creatureIndex++;
            }
        }
    }
}
