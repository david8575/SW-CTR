using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapeStat", menuName = "Shape/ShapeStat")]
public class ShapeStat : ScriptableObject
{
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField]
    private float maxSpeed;
    public float MaxSpeed { get { return maxSpeed; } }

    [SerializeField]
    private float jumpPower;
    public float JumpPower { get { return jumpPower; } }

    [SerializeField]
    private float attack;
    public float Attack { get { return attack; } }

    [SerializeField]
    private float defense;
    public float Defense { get { return defense; } }

    [SerializeField]
    private float cooldown;
    public float Cooldown { get { return cooldown; } }

    [SerializeField]
    private string prefabName;
    public string PrefabName { get { return prefabName; } }

}
