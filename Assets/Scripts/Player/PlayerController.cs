using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    Rigidbody2D rb;

    public float speed = 10;
    public float maxSpeed = 100;
    public float jumpPower = 10;
    float moveInput;
    public bool canMove = true;

    public Shape shapeInfo = null;

    public CinemachineVirtualCamera vcam;

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

    /// <summary>
    /// 도형의 종류를 바꾸는 함수
    /// </summary>
    /// <param name="shapeName"> 도형의 이름</param>
    public void SetShapeType(string shapeName)
    {
        if (shapeInfo != null && shapeInfo.name == shapeName)
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

        if (canMove & moveInput != 0)
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
        if (shapeInfo.canJump)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            shapeInfo.canJump = false;
        }

    }

    void OnSpecialStarted(InputAction.CallbackContext context) => shapeInfo.OnSpecialStarted();

    void OnSpecialCanceled(InputAction.CallbackContext context) => shapeInfo.OnSpecialCanceled();

}
