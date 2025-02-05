using System.Collections;
using UnityEngine;

public class Periko : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        StartCoroutine(randomAnimTimer());
    }

    private IEnumerator randomAnimTimer()
    {
        while (true)
        {
            PickRandomAnim();
            yield return new WaitForSeconds(Random.Range(30f, 75f));
        }
    }

    private void PickRandomAnim()
    {
        int r = Random.Range(0, 2);
        if (r == 0) animator.SetTrigger("Idle");
        else animator.SetTrigger("Flight1");
    }
}
