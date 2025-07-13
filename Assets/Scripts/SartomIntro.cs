using UnityEngine;

public class SartomIntro : MonoBehaviour
{
    public Animator animator;

    public void CallTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
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
