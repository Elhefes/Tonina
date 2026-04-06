using UnityEngine;

public class ClickerMaterial : MonoBehaviour
{
    public Material clickerMat;
    public float alpha = 1f;

    Color matColor;

    private void Start()
    {
        alpha = 1f;
        matColor = clickerMat.color;
    }

    private void FixedUpdate()
    {
        if (alpha > 0f)
        {
            alpha -= 0.01f;
            alpha = Mathf.Max(alpha, 0f); // ensures it never goes below 0
        }

        matColor.a = alpha;
        clickerMat.color = new Color(
            clickerMat.color.r,
            clickerMat.color.g,
            clickerMat.color.b,
            alpha
        );
    }
}
