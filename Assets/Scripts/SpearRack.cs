using UnityEngine;

public class SpearRack : Placeable
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
