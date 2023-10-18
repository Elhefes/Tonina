using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthIndicator : MonoBehaviour
{
    public Image healthFill;
    public TMP_Text healthText;

    public void UpdateHealthIndicator(int health)
    {
        healthFill.fillAmount = health * 0.01f;
        healthText.text = health + "";
    }
}
