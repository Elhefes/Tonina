using UnityEngine;
using UnityEngine.UI;

public class SaveManagementScreen : MonoBehaviour
{
    public Button loadSaveButton;
    public Button deleteSaveButton;
    public GameObject[] selectionCircles;
    private int currentSelectionIndex;

    private void OnEnable()
    {
        int selectedSave = PlayerPrefs.GetInt("SelectedSaveFile", 1);
        SelectThisSave(selectedSave);
        UpdateSelectedCircle(selectedSave);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { gameObject.SetActive(false); }
    }

    public void SelectThisSave(int i)
    {
        currentSelectionIndex = i;
        if (i == PlayerPrefs.GetInt("SelectedSaveFile", 1)) loadSaveButton.interactable = false;
        else loadSaveButton.interactable = true;
        UpdateButtonPositions(i);
    }

    public void LoadSaveFile()
    {
        PlayerPrefs.SetInt("SelectedSaveFile", currentSelectionIndex);
        loadSaveButton.interactable = false;
        UpdateSelectedCircle(currentSelectionIndex);
    }

    void UpdateSelectedCircle(int i)
    {
        foreach (GameObject obj in selectionCircles)
        {
            obj.SetActive(false);
        }
        selectionCircles[i - 1].SetActive(true);
    }

    void UpdateButtonPositions(int i)
    {
        if (i == 3)
        {
            loadSaveButton.transform.localPosition = new Vector3(580, -296, 0);
            deleteSaveButton.transform.localPosition = new Vector3(580, -380, 0);
        }
        else if (i == 2)
        {
            loadSaveButton.transform.localPosition = new Vector3(0, -296, 0);
            deleteSaveButton.transform.localPosition = new Vector3(0, -380, 0);
        }
        else
        {
            loadSaveButton.transform.localPosition = new Vector3(-580, -296, 0);
            deleteSaveButton.transform.localPosition = new Vector3(-580, -380, 0);
        }
    }
}
