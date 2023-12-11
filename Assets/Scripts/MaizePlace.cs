using UnityEngine;

public class MaizePlace : MonoBehaviour
{
    public int maizeInPlace;
    public GameObject maizeInBarrel;

    public void GetMaizeFromPlace()
    {
        if (maizeInPlace < 1)
        {
            return;
        }
        maizeInPlace--;
        UpdateMaizeInBarrel();
    }

    private void UpdateMaizeInBarrel()
    {
        if (maizeInPlace < 1)
        {
            maizeInBarrel.SetActive(false);
            return;
        }
        maizeInBarrel.transform.localPosition -= new Vector3(0f, 0.2f, 0f);
    }
}
