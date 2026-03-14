using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackModeValues : MonoBehaviour
{
    public int[] attackRewards;

    public MusicPlayer musicPlayer;
    public GameObject musicSelectionObject;
    public GameObject randomMusicSelectionObject;
    public Image randomMusicToggleImage;
    public Sprite checkMarkOn;
    public Sprite checkMarkOff;
    public TMP_Text chosenSong;
    private int battleSongID;
    private bool battleSongRandomized;

    public TMP_Text maizeAmount;

    private void OnEnable()
    {
        battleSongID = PlayerPrefs.GetInt("battleSongID", 0);
        battleSongRandomized = PlayerPrefs.GetInt("battleSongRandomized", 1) == 1 ? true : false;
        SetMusicSelectionVisuals(battleSongRandomized);
        maizeAmount.text = GameState.Instance.progressionData.maizeProductionLevel.ToString();
    }

    private void SetMusicSelectionVisuals(bool musicIsRandomized)
    {
        if (musicIsRandomized)
        {
            musicSelectionObject.SetActive(false);
            randomMusicSelectionObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            randomMusicToggleImage.sprite = checkMarkOn;
        }
        else
        {
            musicSelectionObject.SetActive(true);
            randomMusicSelectionObject.transform.localPosition = new Vector3(0f, -70f, 0f);
            randomMusicToggleImage.sprite = checkMarkOff;
            UpdateBattleSong();
        }
    }

    public void ToggleRandomBattleMusic()
    {
        if (PlayerPrefs.GetInt("battleSongRandomized", 1) == 1)
        {
            PlayerPrefs.SetInt("battleSongRandomized", 0);
        }
        else
        {
            PlayerPrefs.SetInt("battleSongRandomized", 1);
        }
        SetMusicSelectionVisuals(PlayerPrefs.GetInt("battleSongRandomized", 1) == 1);
    }

    private void UpdateBattleSong()
    {
        chosenSong.text = musicPlayer.battleSongs[battleSongID].name;
        PlayerPrefs.SetInt("battleSongID", battleSongID);
    }

    public void NextBattleSong()
    {
        if (battleSongID < musicPlayer.battleSongs.Count - 1) battleSongID++;
        else battleSongID = 0;
        UpdateBattleSong();
    }

    public void PreviousBattleSong()
    {
        if (battleSongID > 0) battleSongID--;
        else battleSongID = musicPlayer.battleSongs.Count - 1;
        UpdateBattleSong();
    }
}
