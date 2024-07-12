using System.Collections;
using UnityEngine;

public class SpearWarriorAI : EnemyAI
{
    public GameObject spear;

    private Coroutine AI_ControllerCoroutine;

    void Awake()
    {
        AI_ControllerCoroutine = StartCoroutine(PeriodicalTargetChecking());
    }

    protected override IEnumerator PeriodicalTargetChecking()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            while (true)
            {
                if (spear.activeSelf)
                {
                    if (NearestTarget("ToninaTribe", 30f) != null) break;
                    else if (NearestTarget("Barricade", 8f) != null) break;
                }
                else
                {
                    if (NearestTarget("ToninaTribe", 10f) != null) break;
                    else if (NearestTarget("Barricade", 8f) != null) break;
                    else if (NearestTarget("ToninaTribe", 20f) != null) break;
                }
                creatureMovement.target = GameObject.FindGameObjectsWithTag("Target")[0].transform;
                break;
            }
        }
    }
}
