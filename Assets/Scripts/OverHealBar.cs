using UnityEngine;

public class OverHealBar : MonoBehaviour
{
    public GameObject overHealBar;

    public void UpdateOverHealBar(int health, int startingHealth)
    {
        if (health < startingHealth) health = startingHealth;
        this.transform.localScale = new Vector3((health - startingHealth) * 0.01f, 1f, 1f);
    }
}
