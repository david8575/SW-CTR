using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
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


    PlayerInputActions playerInputActions;
    Rigidbody2D rb;

    public int hp = 100;

    public float speed = 10;
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

    public CinemachineVirtualCamera vcam;
    public Image image;

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

    private void Start()
    {
        // 키 입력과 함수 연결
        #region key Input
        playerInputActions = new();
        playerInputActions.Enable();

        playerInputActions.PlayerActions.Move.started += (x) => moveInput = x.ReadValue<float>();
        playerInputActions.PlayerActions.Move.canceled += (x) => moveInput = x.ReadValue<float>();

        playerInputActions.PlayerActions.Jump.performed += OnJump;

        playerInputActions.PlayerActions.ChangeCircle.performed += (x) => SetShapeType("Circle");
        playerInputActions.PlayerActions.ChangeSquare.performed += (x) => SetShapeType("Square");
        playerInputActions.PlayerActions.ChangeTriangle.performed += (x) => SetShapeType("Triangle");

        playerInputActions.PlayerActions.Special.started += OnSpecialStarted;
        playerInputActions.PlayerActions.Special.canceled += OnSpecialCanceled;
        #endregion

        SetShapeType("Circle");
    }

    // 도형의 종류를 바꾸는 함수
    public void SetShapeType(string shapeName)
    {
        if (ShapeInfo != null && ShapeInfo.name == shapeName || isAttacking == true)
            return;

        // 도형 로드
        string shapeStatPath = "Player/" + shapeName;
        var newShapeInfo = Resources.Load<Shape>(shapeStatPath);

        // 자주쓰는 능력치 저장
        speed = newShapeInfo.speed;
        maxSpeed = newShapeInfo.speed * 2;
        jumpPower = newShapeInfo.jumpForce;

        // 도형 생성
        var newShape = Instantiate(newShapeInfo, transform.position, Quaternion.identity, transform);
        newShape.Init(this);
        Vector3 vel = Vector3.zero;

        if (ShapeInfo != null)
        {
            // 도형 정보 이동 및 예전 도형 삭제
            vel = rb.velocity;
            newShape.transform.position = ShapeInfo.transform.position;

            DestroyImmediate(ShapeInfo.gameObject);
        }

        // 도형 정보 저장
        ShapeInfo = newShape;

        rb = newShape.GetComponent<Rigidbody2D>();
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

    // 점프 키를 눌렀을 때
    void OnJump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            canJump = false;
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

    // 스페셜 능력은 도형에게 맡기기
    void OnSpecialStarted(InputAction.CallbackContext context) => ShapeInfo.OnSpecialStarted();

    void OnSpecialCanceled(InputAction.CallbackContext context)
    {
        ShapeInfo.OnSpecialCanceled();
        // 쿨타임은 여기서 돌리기
        StartCoroutine(WaitSpecialCooldown(ShapeInfo.cooldown));
    }

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

}
