using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthIndicator : MonoBehaviour
{
    public Image healthFill;
    public TMP_Text healthText;

    public void UpdateHealthIndicator(int health, int startingHealth)
    {
        healthFill.fillAmount = (float) health / startingHealth;
        healthText.text = health + "";
    }
}
