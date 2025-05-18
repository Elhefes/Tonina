using System.Collections;
using UnityEngine;
using TMPro;

public class LosingScreen : MonoBehaviour
{
    public TMP_Text youLoseText;
    public TMP_Text reasonText;
    public TMP_Text tryAgainText;
    public Animator backgroundAnimator;
    public Player player;
    public BuildingsManager buildingsManager;
    public MouseLook mouseLook;
    public AudioPassController audioPassController;
    public bool playerDied;
    public GameObject kingBarricade;
    private Coroutine textTypingCoroutine;
    private string fullText;
    private string currentText = "";

    private void OnEnable()
    {
        player.battlefieldMenu.waveController.battleIsLost = true;
        textTypingCoroutine = StartCoroutine(TypeTexts());
        audioPassController.muffleEffect = true;
    }

    public void SetPlayerDied(bool value) { playerDied = value; }

    void SetReasonText()
    {
        if (playerDied) reasonText.text = "King Sartom has died.";
        else reasonText.text = "The enemy reached\nKing Sartom's house.";
    }

    private IEnumerator TypeTexts()
    {
        fullText = youLoseText.text;
        youLoseText.text = "";

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            youLoseText.text = currentText;
        }

        buildingsManager.DestroyPlacedBuildings(); // Destroy all placed buildings in battlefield
        buildingsManager.buildingsPlaced = 0;

        SetReasonText();
        yield return new WaitForSeconds(1.5f);

        fullText = reasonText.text;
        reasonText.text = "";
        reasonText.gameObject.SetActive(true);

        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
            reasonText.text = currentText;
        }

        yield return new WaitForSeconds(1.5f);

        fullText = tryAgainText.text;
        tryAgainText.text = "";
        tryAgainText.gameObject.SetActive(true);

        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
            tryAgainText.text = currentText;
        }

        audioPassController.muffleEffect = false;
        player.gameObject.SetActive(true);
        player.LoseBattle();
        player.EndBattle();
        player.ReturnHome(null);
        mouseLook.ToggleCameraOnPlayer();
        if (!playerDied) kingBarricade.SetActive(true);
        yield return new WaitForSeconds(1f);
        youLoseText.gameObject.SetActive(false);
        reasonText.gameObject.SetActive(false);
        tryAgainText.gameObject.SetActive(false);
        backgroundAnimator.SetTrigger("FadeFromBlack");
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
        youLoseText.gameObject.SetActive(true);
    }
}
