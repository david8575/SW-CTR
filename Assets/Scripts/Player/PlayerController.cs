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

    // 도형에 저장하면 도형 바꿀 시 없어짐   
    // 점프가 가능한지
    public bool canJump = true;
    // 특수 능력 사용이 가능한지
    public bool canSpecial = true;
    // 적과 부딫히면 공격인지 피해 받음인지
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
        // 키 입력과 함수 연결
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

    // 능력치에 업그레이드 배율 적용
    void SetStat()
    {
        GameData data = DataManager.Instance.SaveData;
        attack += attack * ((float)data.attackStat / GameData.maxStatPoint);
        defense += defense * ((float)data.defenseStat / GameData.maxStatPoint);
        speed += speed * ((float)data.speedStat / GameData.maxStatPoint);
        cooldown -= cooldown * ((float)data.cooldownStat / GameData.maxStatPoint);
           
    }

    // 도형의 종류를 바꾸는 함수
    public void SetShapeType(ShapeType shapeType)
    {
        if (CanChangeShape[(int)shapeType] == false)
        {
            return;
        }

        int idx = (int)shapeType;
        // ShapeInfo가 null 이 아닐때 감지해야하므로 &&로 묶어줌
        if (ShapeInfo != null && ShapeInfo.name == shapes[idx].name && ShapeInfo.IsInvincible == false
            // 이렇게 true로 감지하는게 명확해서 가능하면 ||로 뺴줌
            || isAttacking == true 
            )
        {
            Debug.Log("이미 선택된 도형");
            return;
        }

        uiManager.UpdateShape(idx);

        // 도형 로드
        //string shapeStatPath = "Player/" + shapeName;
        //var newShapeInfo = Resources.Load<Shape>(shapeStatPath);

        Shape newShapeInfo = shapes[idx];

        // 자주쓰는 능력치 저장
        speed = newShapeInfo.speed;
        attack = newShapeInfo.attack;
        defense = newShapeInfo.defense;
        maxSpeed = newShapeInfo.speed * 2;
        jumpPower = newShapeInfo.jumpForce;
        cooldown = newShapeInfo.cooldown;
        SetStat();

        // 도형 생성
        //var newShape = Instantiate(newShapeInfo, transform.position, Quaternion.identity, transform);
        //newShape.Init(this);
        Vector3 vel = Vector3.zero;
        newShapeInfo.gameObject.SetActive(true);

        if (ShapeInfo != null)
        {
            // 도형 정보 이동 및 예전 도형 비활성화
            vel = rb.velocity;
            newShapeInfo.transform.position = ShapeInfo.transform.position;

            ShapeInfo.gameObject.SetActive(false);
            //DestroyImmediate(ShapeInfo.gameObject);
        }

        // 도형 정보 저장
        ShapeInfo = newShapeInfo;

        rb = newShapeInfo.rb;
        rb.velocity = vel;
        vcam.Follow = ShapeInfo.transform;

        Debug.Log("도형 변경 완료");
    }

    private void FixedUpdate()
    {
        // 좌우 이동
        if (moveInput != 0)
        {
            if (moveInput > 0)
            {
                // 만약 최대 속도보다 크다면 속도를 증가하지 않음
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

    // 특수 능력 쿨타임
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

        Debug.Log("쿨타임 종료");
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
    // 점프 키를 눌렀을 때
    void OnJump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            canJump = false;
        }

    }
    // 스페셜 능력은 도형에게 맡기기
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
        // 쿨타임은 여기서 돌리기
        specialCooldownCoroutine = StartCoroutine(WaitSpecialCooldown(ShapeInfo.cooldown));
    }

    void OnEnterPerformd(InputAction.CallbackContext context)
    {
        // ClearPoint에서 누르면 승리
        GameManager.Instance.CurrentStage.CheckPlayerInClearPoint();
    }

    #endregion

    // 조작 활성화/비활성화
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

    // 도형의 위치 반환
    public Transform GetShapeTransform() => ShapeInfo.transform;

    // 도형 사망
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

        Debug.Log("피격 " + damage + " 데미지");

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
