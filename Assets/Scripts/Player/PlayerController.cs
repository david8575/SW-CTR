using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
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


    PlayerInputActions playerInputActions;
    Rigidbody2D rb;

    public int hp = 100;

    public float speed = 10;
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

    public CinemachineVirtualCamera vcam;
    public Image image;

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

    private void Start()
    {
        // Ű �Է°� �Լ� ����
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

    // ������ ������ �ٲٴ� �Լ�
    public void SetShapeType(string shapeName)
    {
        if (ShapeInfo != null && ShapeInfo.name == shapeName || isAttacking == true)
            return;

        // ���� �ε�
        string shapeStatPath = "Player/" + shapeName;
        var newShapeInfo = Resources.Load<Shape>(shapeStatPath);

        // ���־��� �ɷ�ġ ����
        speed = newShapeInfo.speed;
        maxSpeed = newShapeInfo.speed * 2;
        jumpPower = newShapeInfo.jumpForce;

        // ���� ����
        var newShape = Instantiate(newShapeInfo, transform.position, Quaternion.identity, transform);
        newShape.Init(this);
        Vector3 vel = Vector3.zero;

        if (ShapeInfo != null)
        {
            // ���� ���� �̵� �� ���� ���� ����
            vel = rb.velocity;
            newShape.transform.position = ShapeInfo.transform.position;

            DestroyImmediate(ShapeInfo.gameObject);
        }

        // ���� ���� ����
        ShapeInfo = newShape;

        rb = newShape.GetComponent<Rigidbody2D>();
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

    // ���� Ű�� ������ ��
    void OnJump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            canJump = false;
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

    // ����� �ɷ��� �������� �ñ��
    void OnSpecialStarted(InputAction.CallbackContext context) => ShapeInfo.OnSpecialStarted();

    void OnSpecialCanceled(InputAction.CallbackContext context)
    {
        ShapeInfo.OnSpecialCanceled();
        // ��Ÿ���� ���⼭ ������
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
