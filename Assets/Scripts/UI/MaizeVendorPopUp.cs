using UnityEngine;

public class MaizeVendorPopUp : MonoBehaviour
{
    public GameObject[] textObjects;
    public MaizeHandler maizeHandler;
    private int startingMaizeAmount;

    public void BuyMoreStartingMaize()
    {
        if (startingMaizeAmount >= textObjects.Length) return;

            textObjects[startingMaizeAmount].SetActive(false);

        startingMaizeAmount++;
        maizeHandler.startingMaize = startingMaizeAmount;

        if (startingMaizeAmount < textObjects.Length) textObjects[startingMaizeAmount].SetActive(true);
    }
}
