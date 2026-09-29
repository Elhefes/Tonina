using UnityEngine;

public class CamazoCave : MonoBehaviour
{
    public Camazo camazoInCave;
    public Transform camazoSpawnPosition;
    public Vector3 camazoSpawnRotation;

    public bool camazoIsSummoned;

    private int bonesOnTablesAmount;
    public GameObject[] bonesOnTables;

    public GameObject coinStack1;
    public GameObject coinStack2;
    public GameObject coinStack3;

    public void DisappearCamazo()
    {
        camazoInCave.ResetAnimatorStates();
        camazoInCave.animator.SetTrigger("Empty");
        camazoInCave.gameObject.SetActive(false);
        camazoInCave.transform.position = camazoSpawnPosition.position;
        camazoInCave.gameObject.SetActive(true);
        camazoIsSummoned = false;
    }

    public void SummonCamazo()
    {
        if (!camazoIsSummoned)
        {
            camazoInCave.animator.SetTrigger("FlyToCaveBridge");
            camazoIsSummoned = true;
        }
    }
}
