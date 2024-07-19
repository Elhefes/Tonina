using System.Collections;
using UnityEngine;

public class BirdShadowSpawner : MonoBehaviour
{
    private Coroutine spawnerCoroutine;
    public GameObject[] shadowObjects;
    public GameObject[] shadowChildren;

    void OnEnable()
    {
        spawnerCoroutine = StartCoroutine(SpawnRandomShadowAtRandomZ());
    }

    private IEnumerator SpawnRandomShadowAtRandomZ()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            int r = Random.Range(0, shadowObjects.Length);
            if (shadowObjects[r].activeSelf)
            {
                if (shadowChildren[r].transform.position.x > 180)
                {
                    // Reset animation
                    shadowObjects[r].SetActive(false);
                    shadowObjects[r].SetActive(true);

                    shadowObjects[r].transform.localPosition = new Vector3(0f, 0f, Random.Range(-40f, 30f));
                }
            }
            else
            {
                shadowObjects[r].SetActive(true);
                shadowObjects[r].transform.localPosition = new Vector3(0f, 0f, Random.Range(-40f, 30f));
            }
        }
    }
}
