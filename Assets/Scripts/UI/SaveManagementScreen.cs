using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

public class SaveManagementScreen : MonoBehaviour
{
    public TMP_Text save1Name;
    public TMP_Text save2Name;
    public TMP_Text save3Name;

    public Image save1Image;
    public Image save2Image;
    public Image save3Image;

    public Sprite saveExistsSprite;
    public Sprite saveDoesNotExistSprite;

    public TMP_Text loadingTextObject;

    public Button closeMenuButton;
    public Button[] saveFileButtons;

    public Button loadSaveButton;
    public Button deleteSaveButton;
    public GameObject[] selectionCircles;
    private int currentSelectionIndex;

    private bool loadingSaveFile;

    public GameObject skipIntroButton;

    private void OnEnable()
    {
        int selectedSave = PlayerPrefs.GetInt("selectedSaveFile", 1);
        SelectThisSave(selectedSave);
        UpdateSelectedCircle(selectedSave);
        LoadSaveVisuals();

#if UNITY_EDITOR
        skipIntroButton.SetActive(true);
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !loadingSaveFile) { gameObject.SetActive(false); }
    }

    private void LoadSaveVisuals()
    {
        save1Name.text = PlayerPrefs.GetString("playerName1", "");
        save2Name.text = PlayerPrefs.GetString("playerName2", "");
        save3Name.text = PlayerPrefs.GetString("playerName3", "");

        if (File.Exists(Application.persistentDataPath + "/world1Data.imox")) save1Image.sprite = saveExistsSprite;
        else save1Image.sprite = saveDoesNotExistSprite;
        if (File.Exists(Application.persistentDataPath + "/world2Data.imox")) save2Image.sprite = saveExistsSprite;
        else save2Image.sprite = saveDoesNotExistSprite;
        if (File.Exists(Application.persistentDataPath + "/world3Data.imox")) save3Image.sprite = saveExistsSprite;
        else save3Image.sprite = saveDoesNotExistSprite;
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
        StartCoroutine(RestartThisScene(true));
    }

    public void DeleteThisSave()
    {
        GameState.Instance.DeleteWorld(currentSelectionIndex);
        LoadSaveVisuals();
        // Lock and restart scene when current save is deleted
        if (currentSelectionIndex == PlayerPrefs.GetInt("selectedSaveFile", 1)) StartCoroutine(RestartThisScene(false));
    }

    private IEnumerator RestartThisScene(bool isOtherSave)
    {
        loadingSaveFile = true;
        if (!isOtherSave) loadingTextObject.text = "Deleting...";
        loadingTextObject.gameObject.SetActive(true);

        closeMenuButton.interactable = false;
        deleteSaveButton.interactable = false;
        foreach (Button button in saveFileButtons) button.interactable = false;
        saveFileButtons[currentSelectionIndex - 1].interactable = true;

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateSelectedCircle(int i)
    {
        foreach (GameObject obj in selectionCircles)
        {
            obj.SetActive(false);
        }
        selectionCircles[i - 1].SetActive(true);
    }

    private void UpdateButtonPositions(int i)
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

    public void SkipIntroRequirements()
    {
        GameState.Instance.progressionData.introPlayed = true;
        GameState.Instance.SaveWorld();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
