using UnityEngine;

public class BuildModeGuide : MonoBehaviour
{
    public GameObject slide1;
    public GameObject slide2;
    public GameObject darkener;
    public GameObject postText;

    public PlacedObjectsGrid placedObjectsGrid;

    public void AutoStart()
    {
        if (PlayerPrefs.GetInt("BuildModeGuided", 0) != 1)
        {
            StartGuide();
        }
        else
        {
            slide1.SetActive(false);
            slide2.SetActive(false);
            darkener.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void StartGuide()
    {
        if (placedObjectsGrid.AnyElementIsActive()) slide1.SetActive(true);
        else slide2.SetActive(true);
        darkener.SetActive(true);
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
