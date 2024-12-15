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
    public Image[] disables;
    Color saveColor;

    PlayerController controller;

    public void Init(PlayerController player)
    {
        controller = player;
        saveColor = shapes[0].color;

        for (int i = 0; i < disables.Length; i++)
        {
            disables[i].gameObject.SetActive(!controller.CanChangeShape[i]);
        }

        SetHp(player.hp, player.maxHp);
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

    public void disableShape(int idx, bool OnOff = true)
    {
        disables[idx].gameObject.SetActive(OnOff);
    }
}
