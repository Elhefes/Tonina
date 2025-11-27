using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveManagementScreen : MonoBehaviour
{
    public TMP_Text save1Name;
    public TMP_Text save2Name;
    public TMP_Text save3Name;

    public Button loadSaveButton;
    public Button deleteSaveButton;
    public GameObject[] selectionCircles;
    private int currentSelectionIndex;

    private void OnEnable()
    {
        int selectedSave = PlayerPrefs.GetInt("selectedSaveFile", 1);
        SelectThisSave(selectedSave);
        UpdateSelectedCircle(selectedSave);
        LoadPlayerNames();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { gameObject.SetActive(false); }
    }

    private void LoadPlayerNames()
    {
        save1Name.text = PlayerPrefs.GetString("playerName1", "");
        save2Name.text = PlayerPrefs.GetString("playerName2", "");
        save3Name.text = PlayerPrefs.GetString("playerName3", "");
    }

    public void SelectThisSave(int i)
    {
        currentSelectionIndex = i;
        if (i == PlayerPrefs.GetInt("selectedSaveFile", 1)) loadSaveButton.interactable = false;
        else loadSaveButton.interactable = true;
        UpdateButtonPositions(i);
    }

    public void LoadSaveFile()
    {
        PlayerPrefs.SetInt("selectedSaveFile", currentSelectionIndex);
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
