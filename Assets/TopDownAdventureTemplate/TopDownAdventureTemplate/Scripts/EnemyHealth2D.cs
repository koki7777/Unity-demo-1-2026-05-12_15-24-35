using System;
using UnityEngine;

/// <summary>
/// 敵のHP、被ダメージ、死亡処理を管理します。
/// </summary>
[RequireComponent(typeof(EnemyController2D))]
public class EnemyHealth2D : MonoBehaviour, IDamageable2D
{
    [Header("HP")]
    [SerializeField] private int maxHP = 30;

    [Header("Death")]
    [SerializeField] private float destroyDelay = 0f;

    private int currentHP;
    private bool isDead;
    private EnemyController2D enemyController;
    private Collider2D[] colliders;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    public event Action<EnemyHealth2D> EnemyDied;

    private void Awake()
    {
        currentHP = maxHP;
        enemyController = GetComponent<EnemyController2D>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce, GameObject attacker)
    {
        if (isDead) return;

        currentHP = Mathf.Max(0, currentHP - Mathf.Max(0, damage));
        enemyController.ApplyKnockback(knockbackDirection, knockbackForce);

        if (currentHP <= 0)
        {
            Die(attacker);
        }
    }

    private void Die(GameObject attacker)
    {
        if (isDead) return;
        isDead = true;

        enemyController.SetCanMove(false);

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        PlayerProgression2D progression = attacker != null ? attacker.GetComponent<PlayerProgression2D>() : null;
        if (progression != null)
        {
            progression.RegisterEnemyDefeated();
        }

        EnemyDied?.Invoke(this);
        Destroy(gameObject, destroyDelay);
    }
}
