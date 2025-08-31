using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public CreatureMovement creatureMovement;
    private Coroutine AI_ControllerCoroutine;

    void Awake()
    {
        AI_ControllerCoroutine = StartCoroutine(PeriodicalTargetChecking());
    }

    protected virtual IEnumerator PeriodicalTargetChecking()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            while (true)
            {
                if (NearestTarget("ToninaTribe", 8f) != null) break;
                else if (NearestTarget("Barricade", 10f) != null) break;
                else if (NearestTarget("ToninaTribe", 20f) != null) break;
                else
                {
                    GameObject[] g = GameObject.FindGameObjectsWithTag("Target");
                    if (g.Length > 0) creatureMovement.target = g[0].transform;
                }
                break;
            }
        }
    }

    public GameObject NearestTarget(string tag, float range)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in objectsWithTag)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance < nearestDistance && distance <= range)
            {
                nearestObject = obj;
                nearestDistance = distance;
            }
        }

        if (nearestObject != null) creatureMovement.target = nearestObject.transform;
        return nearestObject;
    }
}
