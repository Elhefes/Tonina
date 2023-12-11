using UnityEngine;

public class MaizePlace : MonoBehaviour
{
    public int maizeInPlace;

    public void GetMaizeFromPlace()
    {
        if (maizeInPlace < 1)
        {
            return;
        }
        maizeInPlace--;
    }
}
