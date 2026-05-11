using UnityEngine;

public class ChichenItzaMapProgression : MonoBehaviour
{
    public GameObject player;

    // Pickables
    public GameObject pyramidKey;
    public GameObject pickableCamazoIcon;
    public GameObject pickableMask;
    public GameObject pickableKukulkans;

    // Moving elements
    public GameObject camazoSouthWall;
    public GameObject camazoEastWall;

    private void Update()
    {
        if (!pyramidKey.activeInHierarchy)
        {
            if (camazoSouthWall.activeInHierarchy)
            {
                if (Vector3.Distance(player.transform.position, camazoSouthWall.transform.position) < 2f) camazoSouthWall.SetActive(false);
            }
            else if (camazoEastWall.activeInHierarchy)
            {
                if (Vector3.Distance(player.transform.position, camazoEastWall.transform.position) < 2f) camazoEastWall.SetActive(false);
            }
        }
    }
}