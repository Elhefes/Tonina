using UnityEngine;

public class SartomIntro : MonoBehaviour
{
    public Animator animator;
    public GameObject playerPivot;
    public GameObject staticTwinStatue;

    public void CallTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void EnableOnlyBattleModeObjects()
    {
        playerPivot.SetActive(true);
        staticTwinStatue.SetActive(true);
        gameObject.SetActive(false);
    }

    void LegsTimingTrigger()
    {
        animator.SetTrigger("LegsTiming");
    }

    void ResetTrigger(string trigger)
    {
        animator.ResetTrigger(trigger);
    }
}
