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
    public PlaceablesManager buildingsManager;
    public MouseLook mouseLook;
    public AudioPassController audioPassController;
    public bool playerDied;
    public GameObject kingBarricade;
    private string fullText;
    private string currentText = "";

    private void OnEnable()
    {
        if (player.battlefieldMenu != null) player.battlefieldMenu.waveController.battleIsLost = true;
        StartCoroutine(TypeTexts());
        audioPassController.muffleEffect = true;
    }

    public void SetPlayerDied(bool value) { playerDied = value; }

    void SetReasonText()
    {
        if (playerDied) reasonText.text = "King " + PlayerProfile.playerName + " has died.";
        else reasonText.text = "The enemy reached\nKing " + PlayerProfile.playerName + "'s house.";
    }

    private IEnumerator TypeTexts()
    {
        player.LoseBattle();
        fullText = youLoseText.text;
        youLoseText.text = "";

        // Potential idea (old):
        //buildingsManager.DestroyPlacedBuildings(); // Destroy all placed buildings in battlefield
        //buildingsManager.buildingsPlaced = 0;

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            youLoseText.text = currentText;
        }

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
        player.EndBattle();
        player.ReturnHome(null);
        if (kingBarricade != null)
        {
            kingBarricade.SetActive(true);
        }
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
