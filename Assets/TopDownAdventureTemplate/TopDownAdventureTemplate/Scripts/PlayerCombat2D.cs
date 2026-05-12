using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの剣攻撃を管理します。
/// 実際の剣画像は表示せず、プレイヤーの正面に扇形の当たり判定を作る方式です。
/// </summary>
public class PlayerCombat2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerController2D playerController;
    [SerializeField] private PlayerProgression2D progression;
    [SerializeField] private Transform attackOrigin;

    [Header("Attack Settings")]
    [SerializeField] private int fallbackAttackDamage = 10;
    [SerializeField] private float attackRange = 1.2f;
    [Range(1f, 360f)]
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackCooldown = 0.35f;
    [SerializeField] private float enemyKnockbackForce = 5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Debug")]
    [SerializeField] private bool drawAttackGizmo = true;

    private float nextAttackTime;
    private readonly HashSet<IDamageable2D> alreadyHitThisSwing = new HashSet<IDamageable2D>();

    private void Awake()
    {
        if (inputReader == null) inputReader = GetComponent<InputReader>();
        if (playerController == null) playerController = GetComponent<PlayerController2D>();
        if (progression == null) progression = GetComponent<PlayerProgression2D>();
        if (attackOrigin == null) attackOrigin = transform;
    }

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.AttackPressed += TryAttack;
        }
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.AttackPressed -= TryAttack;
        }
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;
        PerformSwingAttack();
    }

    private void PerformSwingAttack()
    {
        alreadyHitThisSwing.Clear();

        Vector2 origin = attackOrigin.position;
        Vector2 facing = playerController != null ? playerController.FacingDirection : Vector2.down;
        int damage = progression != null ? progression.CurrentAttack : fallbackAttackDamage;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Vector2 toTarget = (Vector2)hit.bounds.center - origin;
            if (toTarget.sqrMagnitude <= 0.0001f) continue;

            float angleToTarget = Vector2.Angle(facing, toTarget.normalized);
            if (angleToTarget > attackAngle * 0.5f) continue;

            IDamageable2D damageable = hit.GetComponentInParent<IDamageable2D>();
            if (damageable == null) continue;
            if (alreadyHitThisSwing.Contains(damageable)) continue;

            alreadyHitThisSwing.Add(damageable);
            damageable.TakeDamage(damage, toTarget.normalized, enemyKnockbackForce, gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawAttackGizmo) return;

        Transform originTransform = attackOrigin != null ? attackOrigin : transform;
        Vector3 origin = originTransform.position;
        Vector2 facing = Application.isPlaying && playerController != null ? playerController.FacingDirection : Vector2.down;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, attackRange);

        Vector2 leftDirection = Quaternion.Euler(0f, 0f, attackAngle * 0.5f) * facing;
        Vector2 rightDirection = Quaternion.Euler(0f, 0f, -attackAngle * 0.5f) * facing;

        Gizmos.DrawLine(origin, origin + (Vector3)leftDirection.normalized * attackRange);
        Gizmos.DrawLine(origin, origin + (Vector3)rightDirection.normalized * attackRange);
    }
}
