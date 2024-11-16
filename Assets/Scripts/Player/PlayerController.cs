using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    #region Singlton
    // Singleton (��𼭵� ���� ����)
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

    // �̱��� ���
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    #endregion


    PlayerInputActions playerInputActions;
    Rigidbody2D rb;

    public float hp = 100;
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

    public Shape ShapeInfo { get; private set; } = null;

    [SerializeField]
    Shape[] shapes;
    
    public enum ShapeType
    {
        Circle = 0,
        Square,
        Triangle
    }

    public CinemachineVirtualCamera vcam;
    public Image image;



    private void Start()
    {
        // Ű �Է°� �Լ� ����
        #region key Input
        playerInputActions = new();
        playerInputActions.Enable();

        playerInputActions.PlayerActions.Move.started += (x) => moveInput = x.ReadValue<float>();
        playerInputActions.PlayerActions.Move.canceled += (x) => moveInput = x.ReadValue<float>();

        playerInputActions.PlayerActions.Jump.performed += OnJump;

        playerInputActions.PlayerActions.ChangeCircle.performed += (x) => SetShapeType(ShapeType.Circle);
        playerInputActions.PlayerActions.ChangeSquare.performed += (x) => SetShapeType(ShapeType.Triangle);
        playerInputActions.PlayerActions.ChangeTriangle.performed += (x) => SetShapeType(ShapeType.Square);

        playerInputActions.PlayerActions.Special.started += OnSpecialStarted;
        playerInputActions.PlayerActions.Special.canceled += OnSpecialCanceled;
        #endregion

        hp += hp * ((float)DataManager.Instance.SaveData.healthStat / GameData.maxStatPoint);
        SetStat();

        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].Init(this);
        }
        SetShapeType(ShapeType.Circle);

        
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
        int idx = (int)shapeType;
        // ShapeInfo�� null �� �ƴҶ� �����ؾ��ϹǷ� &&�� ������
        if (ShapeInfo != null && ShapeInfo.name == shapes[idx].name && ShapeInfo.IsInvincible == false
            // �̷��� true�� �����ϴ°� ��Ȯ�ؼ� �����ϸ� ||�� ����
            || isAttacking == true 
            )
            return;

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

    }

    private void FixedUpdate()
    {

        // Ư�� �ɷ� ��� ���� ���ο� ���� �̹��� ���� ���� (�ӽ� ����)
        if (canSpecial)
        {
            image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            image.color = new Color(1, 1, 1, 0.5f);
        }

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
        yield return new WaitForSeconds(time);
        Debug.Log("��Ÿ�� ����");
        canSpecial = true;
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
        if (ShapeInfo.IsInvincible  == true)
            return;

        ShapeInfo.OnSpecialCanceled();
        // ��Ÿ���� ���⼭ ������
        StartCoroutine(WaitSpecialCooldown(ShapeInfo.cooldown));
    }

    #endregion

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

    public Transform GetShapeTransform() => ShapeInfo.transform;

    public void ShapeDead()
    {
        ShapeInfo.gameObject.SetActive(false);
        playerInputActions.Disable();
    }
}
