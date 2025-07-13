using UnityEngine;

public class SartomIntro : MonoBehaviour
{
    public Animator animator;

    public void CallTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void SetIsMovingTrue() { animator.SetBool("IsMoving", true); }
    public void SetIsMovingFalse() { animator.SetBool("IsMoving", false); }

    void LegsTimingTrigger()
    {
        animator.SetTrigger("LegsTiming");
    }

    void ResetTrigger(string trigger)
    {
        animator.ResetTrigger(trigger);
    }
}
