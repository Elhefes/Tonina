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

    private void OnEnable()
    {
        HideCamazo();
    }

    public void HideCamazo()
    {

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
