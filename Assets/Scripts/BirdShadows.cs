using System.Collections;
using UnityEngine;

public class BirdShadows : MonoBehaviour
{
    public BirdShadowSpawner[] birdShadowSpawners;
    private float range = 72f;
    private Coroutine sectorEnablerCoroutine;

    void Start()
    {
        EnableIfWithinRange(Camera.main.gameObject.transform.position.z);
        sectorEnablerCoroutine = StartCoroutine(UpdateSectors());
    }

    private IEnumerator UpdateSectors()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 20f));
            if (Camera.main != null) EnableIfWithinRange(Camera.main.gameObject.transform.position.z);
        }
    }

    void EnableIfWithinRange(float value)
    {
        for (int i = 0; i < birdShadowSpawners.Length; i++)
        {
            if (IsWithinRange(value, birdShadowSpawners[i].gameObject.transform.position.z)) birdShadowSpawners[i].gameObject.SetActive(true);
            else
            {
                foreach (GameObject shadowObj in birdShadowSpawners[i].shadowObjects)
                {
                    shadowObj.SetActive(false); // This is to reset their animators so they don't get stuck to where they were disabled
                }
                birdShadowSpawners[i].gameObject.SetActive(false);
            }
        }
    }

    bool IsWithinRange(float value, float target)
    {
        return Mathf.Abs(value - target) <= range;
    }
}
