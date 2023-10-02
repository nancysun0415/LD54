using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 groundDirection;

    private const string STATE = "state";

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpSpeed = 4.6f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState {
        Idle,
        Walk,
        Jump,
        Fall
    }
    private MovementState movementState;

    private enum MovementDirection {
        Left, Right, Up, Down,
    }
    private MovementDirection movementDirection;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        movementState = MovementState.Idle;
        movementDirection = MovementDirection.Down;
        groundDirection = Vector2.down;
    }

    private void Start() {
        GravityManager.Instance.OnGravityChange += GravityManager_OnGravityChange;
    }

    private void GravityManager_OnGravityChange(object sender, System.EventArgs e) {
        if(GravityManager.Instance.GetState() == GravityManager.GravityState.Up) {
            movementDirection = MovementDirection.Up;
        }
        if (GravityManager.Instance.GetState() == GravityManager.GravityState.Down) {
            movementDirection = MovementDirection.Down;
        }
        if (GravityManager.Instance.GetState() == GravityManager.GravityState.Left) {
            movementDirection = MovementDirection.Left;
        }
        if (GravityManager.Instance.GetState() == GravityManager.GravityState.Right) {
            movementDirection = MovementDirection.Right;
        }
    }

    private void SetMovementDirection() {
        Vector2 inputVector = GetMovementVector();

        switch (movementDirection) {
            case MovementDirection.Up:
                rb.velocity = new Vector2(-inputVector.x * moveSpeed, rb.velocity.y);
                groundDirection = Vector2.up;

                if (Input.GetButtonDown("Jump") && IsGrounded()) {
                    rb.velocity = new Vector2(rb.velocity.x, -jumpSpeed);
                }
                break;

            case MovementDirection.Down:
                rb.velocity = new Vector2(inputVector.x * moveSpeed, rb.velocity.y);
                groundDirection = Vector2.down;

                if (Input.GetButtonDown("Jump") && IsGrounded()) {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                }
                break;

            case MovementDirection.Left:
                rb.velocity = new Vector2(rb.velocity.x , -inputVector.x * moveSpeed);
                groundDirection = Vector2.left;

                if (Input.GetButtonDown("Jump") && IsGrounded()) {
                    rb.velocity = new Vector2(jumpSpeed, rb.velocity.y);
                }
                break;

            case MovementDirection.Right:
                rb.velocity = new Vector2(rb.velocity.x, inputVector.x * moveSpeed);
                groundDirection = Vector2.right;

                if (Input.GetButtonDown("Jump") && IsGrounded()) {
                    rb.velocity = new Vector2(-jumpSpeed, rb.velocity.y);
                }
                break;
        }
    }

    private Vector2 GetMovementVector() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void Update() {
        SetMovementDirection();

        UpdateAnimation();
    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f , groundDirection, 0.1f, jumpableGround);
    }

    public void UpdateAnimation() {


        if (GetMovementVector().x > 0f) {
            movementState = MovementState.Walk;
            spriteRenderer.flipX = false;

        } else if (GetMovementVector().x < 0f) {
            movementState = MovementState.Walk;
            spriteRenderer.flipX = true;

        } else
            movementState = MovementState.Idle;

        float jumpDetector = rb.velocity.y;
        switch (movementDirection) {
            case MovementDirection.Up:
                jumpDetector = -rb.velocity.y; break;

            case MovementDirection.Down:
                jumpDetector = rb.velocity.y; break;

            case MovementDirection.Left:
                jumpDetector = rb.velocity.x; break;

            case MovementDirection.Right:
                jumpDetector = -rb.velocity.x; break;
        }

        if (jumpDetector > .1f) {
            movementState = MovementState.Jump;

        }else if(jumpDetector < -.1f) {
            movementState = MovementState.Fall;
        }

        animator.SetInteger(STATE, (int)movementState);
    }
}
