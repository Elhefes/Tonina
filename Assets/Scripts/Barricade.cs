using UnityEngine;

public class Barricade : MonoBehaviour
{
    public int startingHealth;
    private int health;
    public GameObject layerToDisappear;

    private void Start()
    {
        health = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= startingHealth / 2 && layerToDisappear != null) layerToDisappear.SetActive(false);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        print("Barricade health: " + health);
    }
}
