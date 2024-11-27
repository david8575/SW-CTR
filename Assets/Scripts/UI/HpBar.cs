using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public Image hpBar;

    float maxHp;

    public void SetHp(float HP, float MaxHp)
    {
        maxHp = MaxHp;

        SetHp(HP);
    }

    public void SetHp(float HP)
    {
        hpBar.fillAmount = HP / maxHp;
        if (hpText != null)
            hpText.text = HP + " / " + maxHp;
    }
}
