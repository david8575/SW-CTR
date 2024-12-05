using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    [SerializeField]
    private float maxHp = 100;
    public float MaxHp { get { return maxHp; } }

    [SerializeField]
    private float attack = 10;
    public float Attack { get { return attack; } }

    [SerializeField]
    private float defense = 10;
    public float Defense { get { return defense; } }


    [SerializeField]
    private float speed = 10;
    public float Speed { get { return speed; } }


    [SerializeField]
    private float cooldown = 10;
    public float Cooldown { get { return cooldown; } }


    [SerializeField]
    private float maxSpeed = 100;
    public float MaxSpeed { get { return maxSpeed; } }


    [SerializeField]
    private float jumpPower = 10;
    public float JumpPower { get { return jumpPower; } }

    [SerializeField]
    private float specialPower = 10;
    public float SpecialPower { get { return specialPower; } }
}
