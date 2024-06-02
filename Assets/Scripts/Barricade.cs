using UnityEngine;

public class Barricade : MonoBehaviour
{
    public int startingHealth;
    private int health;
    public GameObject layerToDisappear;
    public bool loseWhenDestroyed;
    public LosingScreen losingScreen;

    private void OnEnable()
    {
        health = startingHealth;
        if (!layerToDisappear.activeSelf) layerToDisappear.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= startingHealth / 2 && layerToDisappear != null) layerToDisappear.SetActive(false);
        if (health <= 0)
        {
            if (loseWhenDestroyed)
            {
                losingScreen.gameObject.SetActive(true);
                losingScreen.SetPlayerDied(false);
            }
            gameObject.SetActive(false);
        }
        print("Barricade health: " + health);
    }
}
