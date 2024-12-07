using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    #region Static Instance
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion


    PlayerInputActions playerInputActions;
    public Rigidbody2D rb { get; private set; }
    [Header("Player Stats")]
    public float hp = 100;
    public float maxHp = 100;
    public float attack = 10;
    public float defense = 10;
    public float speed = 10;
    public float cooldown = 10;

    public float maxSpeed = 100;
    public float jumpPower = 10;
    float moveInput;

    // ������ �����ϸ� ���� �ٲ� �� ������   
    // ������ ��������
    public bool canJump = true;
    // Ư�� �ɷ� ����� ��������
    public bool canSpecial = true;
    // ���� �΋H���� �������� ���� ��������
    public bool isAttacking = false;

    public bool IsInvincible = false;

    [SerializeField]
    Shape shapeInfo = null;
    public Shape ShapeInfo { get => shapeInfo; private set => shapeInfo = value; }

    [SerializeField]
    Shape[] shapes;

    public bool[] CanChangeShape = new bool[3] { true, true, false };
    public bool PlayerInputEnable = true;

    public enum ShapeType
    {
        Circle = 0,
        Square,
        Triangle
    }

    public CinemachineVirtualCamera vcam;
    public PlayerUIManager uiManager;

    Coroutine specialCooldownCoroutine;

    private void Start()
    {
        // Ű �Է°� �Լ� ����
        #region key Input
        playerInputActions = new();

        playerInputActions.PlayerActions.Move.started += (x) => moveInput = x.ReadValue<float>();
        playerInputActions.PlayerActions.Move.canceled += (x) => moveInput = x.ReadValue<float>();

        playerInputActions.PlayerActions.Jump.performed += OnJump;

        playerInputActions.PlayerActions.ChangeCircle.performed += (x) => SetShapeType(ShapeType.Circle);
        playerInputActions.PlayerActions.ChangeSquare.performed += (x) => SetShapeType(ShapeType.Triangle);
        playerInputActions.PlayerActions.ChangeTriangle.performed += (x) => SetShapeType(ShapeType.Square);

        playerInputActions.PlayerActions.Special.started += OnSpecialStarted;
        playerInputActions.PlayerActions.Special.canceled += OnSpecialCanceled;

        playerInputActions.PlayerActions.Enter.performed += OnEnterPerformd;

        if (PlayerInputEnable)
        {
            playerInputActions.Enable();
        }
        #endregion
        hp = shapes[0].status.MaxHp;
        hp += hp * ((float)DataManager.Instance.SaveData.healthStat / GameData.maxStatPoint);
        maxHp = hp;

        if (DataManager.Instance.SaveData.UnlockSquare == true)
        {
            CanChangeShape[2] = true;

        }

        uiManager.Init(this);
        SetStat();

        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].Init(this);
        }
        SetShapeType(ShapeType.Circle);

        uiManager.GetComponent<Canvas>().worldCamera = Camera.main;


    }

    // �ɷ�ġ�� ���׷��̵� ���� ����
    void SetStat()
    {
        GameData data = DataManager.Instance.SaveData;
        attack += attack * ((float)data.attackStat / GameData.maxStatPoint);
        defense += defense * ((float)data.defenseStat / GameData.maxStatPoint);
        speed += speed * ((float)data.speedStat / GameData.maxStatPoint);
        cooldown -= cooldown * ((float)data.cooldownStat / GameData.maxStatPoint);
           
    }

    // ������ ������ �ٲٴ� �Լ�
    public void SetShapeType(ShapeType shapeType)
    {
        if (CanChangeShape[(int)shapeType] == false)
        {
            return;
        }

        int idx = (int)shapeType;
        // ShapeInfo�� null �� �ƴҶ� �����ؾ��ϹǷ� &&�� ������
        if (ShapeInfo != null && ShapeInfo.name == shapes[idx].name && ShapeInfo.IsInvincible == false
            // �̷��� true�� �����ϴ°� ��Ȯ�ؼ� �����ϸ� ||�� ����
            || isAttacking == true 
            )
        {
            Debug.Log("�̹� ���õ� ����");
            return;
        }

        uiManager.UpdateShape(idx);

        // ���� �ε�
        //string shapeStatPath = "Player/" + shapeName;
        //var newShapeInfo = Resources.Load<Shape>(shapeStatPath);

        Shape newShapeInfo = shapes[idx];

        // ���־��� �ɷ�ġ ����
        speed = newShapeInfo.speed;
        attack = newShapeInfo.attack;
        defense = newShapeInfo.defense;
        maxSpeed = newShapeInfo.speed * 2;
        jumpPower = newShapeInfo.jumpForce;
        cooldown = newShapeInfo.cooldown;
        SetStat();

        // ���� ����
        //var newShape = Instantiate(newShapeInfo, transform.position, Quaternion.identity, transform);
        //newShape.Init(this);
        Vector3 vel = Vector3.zero;
        newShapeInfo.gameObject.SetActive(true);

        if (ShapeInfo != null)
        {
            // ���� ���� �̵� �� ���� ���� ��Ȱ��ȭ
            vel = rb.velocity;
            newShapeInfo.transform.position = ShapeInfo.transform.position;

            ShapeInfo.gameObject.SetActive(false);
            //DestroyImmediate(ShapeInfo.gameObject);
        }

        // ���� ���� ����
        ShapeInfo = newShapeInfo;

        rb = newShapeInfo.rb;
        rb.velocity = vel;
        vcam.Follow = ShapeInfo.transform;

        Debug.Log("���� ���� �Ϸ�");
    }

    private void FixedUpdate()
    {
        // �¿� �̵�
        if (moveInput != 0)
        {
            if (moveInput > 0)
            {
                // ���� �ִ� �ӵ����� ũ�ٸ� �ӵ��� �������� ����
                if (rb.velocity.x < maxSpeed)
                {
                    rb.AddForce(new Vector2(moveInput * speed, 0));
                }
            }
            else if (moveInput < 0)
            {
                if (rb.velocity.x > -maxSpeed)
                {
                    rb.AddForce(new Vector2(moveInput * speed, 0));
                }
            }
        }

    }

    // Ư�� �ɷ� ��Ÿ��
    protected IEnumerator WaitSpecialCooldown(float time)
    {
        canSpecial = false;
        float t = 0;
        float maxCooldown = time;
        while (t < maxCooldown)
        {
            t += Time.deltaTime;
            uiManager.UpdateCooldown(t, maxCooldown);
            yield return null;
        }
        uiManager.UpdateCooldown(1.0f, 1.0f);

        Debug.Log("��Ÿ�� ����");
        canSpecial = true;
    }

    public void ResetCooltime()
    {
        canSpecial = true;
        uiManager.UpdateCooldown(1.0f, 1.0f);
        if (specialCooldownCoroutine != null)
            StopCoroutine(specialCooldownCoroutine);
    }


    #region PlayerInput Callbacks
    // ���� Ű�� ������ ��
    void OnJump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            canJump = false;
        }

    }
    // ����� �ɷ��� �������� �ñ��
    void OnSpecialStarted(InputAction.CallbackContext context)
    {
        if (canSpecial == false || ShapeInfo.IsInvincible == true)
            return;
        ShapeInfo.OnSpecialStarted();
    }

    void OnSpecialCanceled(InputAction.CallbackContext context)
    {
        if (ShapeInfo.IsInvincible  == true || canSpecial == false)
            return;
        if (ShapeInfo is Circle && ((Circle)ShapeInfo).isCharging == false)
        {
            return;
        }

        ShapeInfo.OnSpecialCanceled();
        // ��Ÿ���� ���⼭ ������
        specialCooldownCoroutine = StartCoroutine(WaitSpecialCooldown(ShapeInfo.cooldown));
    }

    void OnEnterPerformd(InputAction.CallbackContext context)
    {
        // ClearPoint���� ������ �¸�
        GameManager.Instance.CurrentStage.CheckPlayerInClearPoint();
    }

    #endregion

    // ���� Ȱ��ȭ/��Ȱ��ȭ
    public void SetInputSystem(bool OnOff)
    {
        if (OnOff)
        {
            playerInputActions.Enable();
        }
        else
        {
            playerInputActions.Disable();
        }
    }

    // ������ ��ġ ��ȯ
    public Transform GetShapeTransform() => ShapeInfo.transform;

    // ���� ���
    public void ShapeDead()
    {
        ShapeInfo.gameObject.SetActive(false);
        playerInputActions.Disable();

        GameManager.Instance.CurrentStage.Gameover();
    }

    public void TakeDamage(float damage)
    {
        if (IsInvincible)
            return;

        Debug.Log("�ǰ� " + damage + " ������");

        StartCoroutine(ShapeInfo.Invincible(1.0f));

        hp -= damage;
        if (hp <= 0)
        {
            ShapeDead();
        }

        uiManager.UpdateHPBar(hp);
    }

    public void Heal(float heal)
    {
        hp += heal;
        if (hp > maxHp)
            hp = maxHp;

        uiManager.UpdateHPBar(hp);
    }

    private void OnDestroy()
    {
        playerInputActions.Disable();
        playerInputActions.Dispose();
    }
}
