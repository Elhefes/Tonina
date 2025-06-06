using UnityEngine;

public class BuildModeGuide : MonoBehaviour
{
    public GameObject slide1;
    public GameObject darkener;
    public GameObject postText;

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

    private void OnDisable()
    {
        if (PlayerPrefs.GetInt("BuildModeGuided", 0) == 0) postText.SetActive(true);
    }

    public void SetBuildModeGuided()
    {
        PlayerPrefs.SetInt("BuildModeGuided", 1);
    }
}
