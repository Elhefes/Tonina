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
    public Animator feedbackAnimator;
    public WeatherGame weatherGame;

    private void OnEnable()
    {
        feedbackAnimator.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        lineMatcherAnimator.SetInteger("RandomInt", Random.Range(0, 4));
        lineMatcherAnimator.SetTrigger("MoveAgain");
    }

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
        lineMatcherAnimator.SetInteger("RandomInt", Random.Range(0, 4));
        lineMatcherAnimator.SetTrigger("MoveAgain");

        // Feedback
        if (score >= 92.5f) feedbackAnimator.SetTrigger("Green");
        else if (score >= 75f) feedbackAnimator.SetTrigger("Yellow");
        else feedbackAnimator.SetTrigger("Red");
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
