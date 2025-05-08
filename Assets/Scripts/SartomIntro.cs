using UnityEngine;

public class SartomIntro : MonoBehaviour
{
    public Animator animator;

    void LegsTimingTrigger()
    {
        animator.SetTrigger("LegsTiming");
    }

    void ResetTrigger(string trigger)
    {
        animator.ResetTrigger(trigger);
    }
}
