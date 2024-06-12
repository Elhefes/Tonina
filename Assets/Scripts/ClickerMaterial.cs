using UnityEngine;

public class ClickerMaterial : MonoBehaviour
{
    public Material clickerMat;
    public float alpha = 1f;

    Color matColor;

    void Start()
    {
        alpha = 1f;
        matColor = clickerMat.color;
    }

    void FixedUpdate()
    {
        matColor.a = alpha;
        clickerMat.color = new Color(clickerMat.color.r, clickerMat.color.g, clickerMat.color.b, Mathf.Clamp(alpha, 0f, 1f));
        if (alpha > 0f)
        {
            alpha -= 0.01f;
        }
    }
}
