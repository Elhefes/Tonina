using UnityEngine;

public class SpearRack : Placeable
{
    public GameObject[] spears;
    public int currentAmountOfSpears;
    private int originalAmountOfSpears;

    private void Start()
    {
        currentAmountOfSpears = spears.Length;
        originalAmountOfSpears = currentAmountOfSpears;
    }

    public void TakeSpear()
    {
        spears[currentAmountOfSpears - 1].SetActive(false);
        currentAmountOfSpears--;
    }

    public void ResetSpears()
    {
        foreach (GameObject obj in spears) obj.SetActive(true);
        currentAmountOfSpears = originalAmountOfSpears;
    }
}
