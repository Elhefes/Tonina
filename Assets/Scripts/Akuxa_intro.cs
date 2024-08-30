using UnityEngine;

public class Akuxa_intro : MonoBehaviour
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
