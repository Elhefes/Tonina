using UnityEngine;
using UnityEngine.EventSystems;

public class WeatherGameButton : MonoBehaviour, IPointerDownHandler
{
    private float score;
    private float scoreCounter;
    private int clicks;
    private int finalScore;
    public GameObject lineMatcher;
    public Animator lineMatcherAnimator;
    public WeatherGame weatherGame;

    public void RegisterClick()
    {
        clicks++;
        score = 100f - (Vector3.Distance(transform.position, lineMatcher.transform.position) / 2);
        if (score < 0) score = 0;
        scoreCounter += score;
        if (clicks > 7)
        {
            finalScore = Mathf.RoundToInt(scoreCounter / clicks);
            weatherGame.EndGame(finalScore);
        }
        lineMatcherAnimator.SetTrigger("MoveAgain");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RegisterClick();
    }

    public void ResetValues()
    {
        score = 0;
        scoreCounter = 0;
        clicks = 0;
        finalScore = 0;
    }
}
