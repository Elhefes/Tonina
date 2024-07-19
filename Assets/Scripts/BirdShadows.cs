using System.Collections;
using UnityEngine;

public class BirdShadows : MonoBehaviour
{
    public GameObject[] Z_Sectors;
    private float range = 70f;
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
            EnableIfWithinRange(Camera.main.gameObject.transform.position.z);
        }
    }

    void EnableIfWithinRange(float value)
    {
        for (int i = 0; i < Z_Sectors.Length; i++)
        {
            if (IsWithinRange(value, Z_Sectors[i].gameObject.transform.position.z)) Z_Sectors[i].gameObject.SetActive(true);
            else Z_Sectors[i].gameObject.SetActive(false);
        }
    }

    bool IsWithinRange(float value, float target)
    {
        return Mathf.Abs(value - target) <= range;
    }
}
