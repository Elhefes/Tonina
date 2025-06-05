using UnityEngine;

public class BuildModeGuide : MonoBehaviour
{
    public GameObject slide1;
    public GameObject darkener;

    public void AutoStart()
    {
        if (PlayerPrefs.GetInt("BuildModeGuided", 0) != 1)
        {
            slide1.SetActive(true);
            darkener.SetActive(true);
        }
        else
        {
            slide1.SetActive(false);
            darkener.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void SetBuildModeGuided()
    {
        PlayerPrefs.SetInt("BuildModeGuided", 1);
    }
}
