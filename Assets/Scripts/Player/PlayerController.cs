using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    Rigidbody2D rb;

    public float speed = 10;
    public float maxSpeed = 100;
    public float jumpPower = 10;
    float moveInput;

    public ShapeStat shapeStat;
    public GameObject shape = null;

    private void Awake()
    {
        SetShapeType("Circle");

        playerInputActions = new();
        playerInputActions.Enable();

        playerInputActions.PlayerActions.Move.started += (x) => moveInput = x.ReadValue<float>();
        playerInputActions.PlayerActions.Move.canceled += (x) => moveInput = x.ReadValue<float>();

        playerInputActions.PlayerActions.Jump.performed += OnJump;                                                      

        playerInputActions.PlayerActions.ChangeCircle.performed += (x) => SetShapeType("Circle");                                               
        playerInputActions.PlayerActions.ChangeSquare.performed += (x) => SetShapeType("Square");
        playerInputActions.PlayerActions.ChangeTriangle.performed += (x) => SetShapeType("Triangle");
    }

    public void SetShapeType(string shapeName)
    {
        string shapeStatPath = "Player/ShapeInitStat/" + shapeName;
        shapeStat = Resources.Load<ShapeStat>(shapeStatPath);

        speed = shapeStat.Speed;
        maxSpeed = shapeStat.MaxSpeed;
        jumpPower = shapeStat.JumpPower;


        GameObject prefab = Resources.Load<GameObject>("Player/" + shapeStat.PrefabName);
        GameObject newShape = Instantiate(prefab, transform.position, Quaternion.identity, transform);

        if (shape != null)
        {
            newShape.transform.position = shape.transform.position;
            DestroyImmediate(shape);
            shape = null;
        }

        shape = newShape;
        rb = newShape.GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {

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
        if (rb.velocity.y < 0.1f)
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
    }
}
