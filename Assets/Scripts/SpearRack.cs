using UnityEngine;

public class SpearRack : PlaceableBuilding
{
    public GameObject[] spears;
    public int numOfSpearsInRack;

    private void Start()
    {
        numOfSpearsInRack = spears.Length;
    }

    public void TakeSpear()
    {
        spears[numOfSpearsInRack - 1].SetActive(false);
        numOfSpearsInRack--;
    }
}
