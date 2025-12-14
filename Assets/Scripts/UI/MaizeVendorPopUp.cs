using UnityEngine;

public class MaizeVendorPopUp : MonoBehaviour
{
    public GameObject[] textObjects;
    public MaizeHandler maizeHandler;

    public void BuyMoreStartingMaize()
    {
        if (GameState.Instance.progressionData.maizeProductionLevel >= textObjects.Length) return;

        GameState.Instance.progressionData.maizeProductionLevel++;
        int startingMaizeAmount = GameState.Instance.progressionData.maizeProductionLevel;

        foreach (GameObject obj in textObjects) obj.SetActive(false);
        if (startingMaizeAmount < textObjects.Length) textObjects[startingMaizeAmount].SetActive(true);

        SetMaizeProductionLevel(startingMaizeAmount);
        GameState.Instance.SaveWorld();
    }

    private void SetMaizeProductionLevel(int startingMaizeAmount)
    {
        // TODO: Make Maize Place incrementally cheaper also
        if (startingMaizeAmount == 3)
        {
            // MP Price = 120
        }
        else if (startingMaizeAmount == 3)
        {
            // MP Price = 100
        }
    }
}
