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
        if (!GameState.Instance.progressionData.buildModeGuided)
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
        if (!GameState.Instance.progressionData.buildModeGuided) postText.SetActive(true);
    }

    public void SetBuildModeGuided()
    {
        GameState.Instance.progressionData.buildModeGuided = true;
        GameState.Instance.SaveWorld();
    }
}
