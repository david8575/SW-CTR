using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIManager : MonoBehaviour
{
    public HpBar hpBar;

    public Image cooldownCircle;

    public Image[] shapes;
    Color saveColor;

    private void Start()
    {
        saveColor = shapes[0].color;
    }

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

    public void UpdateShape(int shapeIndex)
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].gameObject.SetActive(i == shapeIndex);
        }
    }

    public void SetShapeState(bool state)
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].color = state ? saveColor : Color.red;
        }
    }
}
