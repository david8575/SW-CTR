using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIManager : MonoBehaviour
{
    [Header("HP Bar")]
    public Image hpBar;
    public TextMeshProUGUI hpText;

    [Header("Cooldown")]
    public Image cooldownCircle;

    public void UpdateHPBar(float hp, float maxHp)
    {
        hpBar.fillAmount = hp / maxHp;
        hpText.text = hp + " / " + maxHp;
    }

    public void UpdateCooldown(float cooldown, float maxCooldown)
    {
        cooldownCircle.fillAmount = cooldown / maxCooldown;
    }
}
