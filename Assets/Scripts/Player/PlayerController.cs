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
    // Singleton (어디서든 접근 가능)
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

    // 싱글톤 등록
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

    // 도형에 저장하면 도형 바꿀 시 없어짐   
    // 점프가 가능한지
    public bool canJump = true;
    // 특수 능력 사용이 가능한지
    public bool canSpecial = true;
    // 적과 부딫히면 공격인지 피해 받음인지
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
        // 키 입력과 함수 연결
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
        int idx = (int)shapeType;
        // ShapeInfo가 null 이 아닐때 감지해야하므로 &&로 묶어줌
        if (ShapeInfo != null && ShapeInfo.name == shapes[idx].name && ShapeInfo.IsInvincible == false
            // 이렇게 true로 감지하는게 명확해서 가능하면 ||로 뺴줌
            || isAttacking == true 
            )
            return;

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

    }

    private void FixedUpdate()
    {

        // 특수 능력 사용 가능 여부에 따라 이미지 투명도 조절 (임시 구현)
        if (canSpecial)
        {
            image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            image.color = new Color(1, 1, 1, 0.5f);
        }

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
        yield return new WaitForSeconds(time);
        Debug.Log("쿨타임 종료");
        canSpecial = true;
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
        if (ShapeInfo.IsInvincible  == true)
            return;

        ShapeInfo.OnSpecialCanceled();
        // 쿨타임은 여기서 돌리기
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
