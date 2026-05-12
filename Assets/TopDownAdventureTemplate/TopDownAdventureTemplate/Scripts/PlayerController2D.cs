using UnityEngine;

/// <summary>
/// 見下ろし2D用のプレイヤー移動スクリプトです。
/// WASD/左スティックで自由移動し、最後に入力した方向を「向いている方向」として保持します。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform facingPivot;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float minInputForFacing = 0.05f;

    [Header("Knockback")]
    [SerializeField] private float knockbackDamping = 18f;

    private Rigidbody2D rb;
    private Vector2 facingDirection = Vector2.down;
    private Vector2 knockbackVelocity;

    public Vector2 FacingDirection => facingDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (inputReader == null)
        {
            inputReader = GetComponent<InputReader>();
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = inputReader != null ? inputReader.Move : Vector2.zero;
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        if (moveInput.sqrMagnitude >= minInputForFacing * minInputForFacing)
        {
            facingDirection = moveInput.normalized;
            UpdateFacingPivot();
        }

        Vector2 movementVelocity = moveInput * moveSpeed;
        Vector2 totalVelocity = movementVelocity + knockbackVelocity;

        rb.MovePosition(rb.position + totalVelocity * Time.fixedDeltaTime);
        knockbackVelocity = Vector2.MoveTowards(knockbackVelocity, Vector2.zero, knockbackDamping * Time.fixedDeltaTime);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (direction.sqrMagnitude <= 0.0001f) return;
        knockbackVelocity = direction.normalized * force;
    }

    private void UpdateFacingPivot()
    {
        if (facingPivot == null) return;

        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        facingPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
