using UnityEngine;

public enum EnemyIdleMode
{
    Idle,
    Patrol
}

/// <summary>
/// 敵の待機、巡回、プレイヤー追跡を管理します。
/// 初心者向けに、障害物を回り込むA*ではなく、プレイヤー方向へ直接近づく方式です。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Move Settings")]
    [SerializeField] private EnemyIdleMode idleMode = EnemyIdleMode.Idle;
    [SerializeField] private float moveSpeed = 3.2f;
    [SerializeField] private float detectRange = 4f;
    [SerializeField] private float stopDistance = 0.2f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waypointReachDistance = 0.15f;
    [SerializeField] private bool loopPatrol = true;

    [Header("Knockback")]
    [SerializeField] private float knockbackDamping = 18f;

    private Rigidbody2D rb;
    private Vector2 knockbackVelocity;
    private int patrolIndex;
    private int patrolDirection = 1;
    private bool canMove = true;

    public float DetectRange => detectRange;
    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null) player = playerObject.transform;
        }
    }

    private void FixedUpdate()
    {
        Vector2 desiredVelocity = Vector2.zero;

        if (canMove)
        {
            desiredVelocity = CalculateDesiredVelocity();
        }

        Vector2 totalVelocity = desiredVelocity + knockbackVelocity;
        rb.MovePosition(rb.position + totalVelocity * Time.fixedDeltaTime);
        knockbackVelocity = Vector2.MoveTowards(knockbackVelocity, Vector2.zero, knockbackDamping * Time.fixedDeltaTime);
    }

    private Vector2 CalculateDesiredVelocity()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectRange && distanceToPlayer > stopDistance)
            {
                Vector2 chaseDirection = ((Vector2)player.position - rb.position).normalized;
                return chaseDirection * moveSpeed;
            }
        }

        if (idleMode == EnemyIdleMode.Patrol)
        {
            return CalculatePatrolVelocity();
        }

        return Vector2.zero;
    }

    private Vector2 CalculatePatrolVelocity()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return Vector2.zero;

        Transform targetPoint = patrolPoints[patrolIndex];
        if (targetPoint == null) return Vector2.zero;

        Vector2 toPoint = (Vector2)targetPoint.position - rb.position;

        if (toPoint.magnitude <= waypointReachDistance)
        {
            AdvancePatrolIndex();
            return Vector2.zero;
        }

        return toPoint.normalized * moveSpeed;
    }

    private void AdvancePatrolIndex()
    {
        if (patrolPoints.Length <= 1) return;

        if (loopPatrol)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
        else
        {
            if (patrolIndex == patrolPoints.Length - 1) patrolDirection = -1;
            if (patrolIndex == 0) patrolDirection = 1;
            patrolIndex += patrolDirection;
        }
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (direction.sqrMagnitude <= 0.0001f) return;
        knockbackVelocity = direction.normalized * force;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
