using TMPro;
using UnityEngine;

public class PlayerHeadingHUD : MonoBehaviour
{
    public Player player;
    public GameObject minimapRenderTextureObject;
    public Transform toCamazoCaveTPPos;
    public Transform camazoCaveSpawnPos;
    public TMP_Text headingTMP;
    public float requiredTime = 5f;

    private float standingTime = 0f;
    private bool playerNearby = false;

    void Update()
    {
        if (Vector3.Distance(player.transform.position, toCamazoCaveTPPos.position) < 2.5f)
        {
            headingTMP.gameObject.SetActive(true);

            standingTime += Time.deltaTime;
            headingTMP.text = "Heading to Camazo Cave in " + 
                Mathf.CeilToInt(requiredTime - standingTime).ToString() + "...";

            // Countdown finished
            if (standingTime >= requiredTime)
            {
                headingTMP.gameObject.SetActive(false);
                player.TeleportToCamazoCave(camazoCaveSpawnPos.position);
                minimapRenderTextureObject.SetActive(false);
            }
        }
        else
        {
            headingTMP.gameObject.SetActive(false);
            standingTime = 0f;
        }
    }
}
