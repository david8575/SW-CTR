using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    Rigidbody2D rb;

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

    public Shape shapeInfo = null;

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

        playerInputActions.PlayerActions.ChangeCircle.performed += (x) => SetShapeType("Circle");
        playerInputActions.PlayerActions.ChangeSquare.performed += (x) => SetShapeType("Square");
        playerInputActions.PlayerActions.ChangeTriangle.performed += (x) => SetShapeType("Triangle");

        playerInputActions.PlayerActions.Special.started += OnSpecialStarted;
        playerInputActions.PlayerActions.Special.canceled += OnSpecialCanceled;
        #endregion

        SetShapeType("Circle");
    }

    /// <summary>
    /// ������ ������ �ٲٴ� �Լ�
    /// </summary>
    /// <param name="shapeName"> ������ �̸�</param>
    public void SetShapeType(string shapeName)
    {
        if (shapeInfo != null && shapeInfo.name == shapeName && isAttacking == false)
            return;

        string shapeStatPath = "Player/" + shapeName;
        var newShapeInfo = Resources.Load<Shape>(shapeStatPath);

        speed = newShapeInfo.speed;
        maxSpeed = newShapeInfo.maxSpeed;
        jumpPower = newShapeInfo.jumpForce;

        var newShape = Instantiate(newShapeInfo, transform.position, Quaternion.identity, transform);
        newShape.Init(this);
        Vector3 vel = Vector3.zero;

        if (shapeInfo != null)
        {
            vel = rb.velocity;
            newShape.transform.position = shapeInfo.transform.position;

            DestroyImmediate(shapeInfo.gameObject);
        }

        shapeInfo = newShape;

        rb = newShape.GetComponent<Rigidbody2D>();
        rb.velocity = vel;
        vcam.Follow = shapeInfo.transform;

    }

    private void FixedUpdate()
    {

        if (canSpecial)
        {
            image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            image.color = new Color(1, 1, 1, 0.5f);
        }

        if (moveInput != 0)
        {
            rb.AddForce(new Vector2(moveInput * speed, 0));
        }

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }

    }

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

    void OnSpecialStarted(InputAction.CallbackContext context) => shapeInfo.OnSpecialStarted();

    void OnSpecialCanceled(InputAction.CallbackContext context)
    {
        shapeInfo.OnSpecialCanceled();
        StartCoroutine(WaitSpecialCooldown(shapeInfo.cooldown));
    }



}
