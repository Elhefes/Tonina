using UnityEngine;

public class Barricade : MonoBehaviour
{
    public int startingHealth;
    public int health;
    public GameObject layerToDisappear;
    public GameObject fullObject; // Needed for placeable barricades
    public bool loseWhenDestroyed;
    public LosingScreen losingScreen;

    private void OnEnable()
    {
        RestoreBarricade();
    }

    public virtual void RestoreBarricade()
    {
        health = startingHealth;
        if (!layerToDisappear.activeSelf) layerToDisappear.SetActive(true);
    }

    public virtual void TakeDamage(int damage)
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
            fullObject.SetActive(false);
        }
        print("Barricade health: " + health);
    }
}
