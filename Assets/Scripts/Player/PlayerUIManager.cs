using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIManager : MonoBehaviour
{
    public HpBar hpBar;

    [Header("Cooldown")]
    public Image cooldownCircle;

    public void SetHp(float hp, float maxHp)
    {
        hpBar.SetHp(hp, maxHp);
    }

    public void UpdateHPBar(float hp)
    {
        hpBar.SetHp(hp);
    }

    public void UpdateCooldown(float cooldown, float maxCooldown)
    {
        cooldownCircle.fillAmount = cooldown / maxCooldown;
    }
}
