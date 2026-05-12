using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーのHP、被ダメージ、無敵時間、ノックバック、死亡を管理します。
/// </summary>
[RequireComponent(typeof(PlayerController2D))]
public class PlayerHealth2D : MonoBehaviour, IDamageable2D
{
    [Header("HP")]
    [SerializeField] private int maxHP = 100;

    [Header("Damage Reaction")]
    [SerializeField] private float invincibleDuration = 1f;
    [SerializeField] private float defaultKnockbackForce = 6f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float blinkInterval = 0.08f;

    private PlayerController2D playerController;
    private bool isInvincible;
    private bool isDead;

    public int CurrentHP { get; private set; }
    public int MaxHP => maxHP;

    public event Action<int, int> HPChanged;
    public event Action PlayerDied;

    private void Awake()
    {
        playerController = GetComponent<PlayerController2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        CurrentHP = maxHP;
    }

    private void Start()
    {
        HPChanged?.Invoke(CurrentHP, maxHP);
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce, GameObject attacker)
    {
        if (isDead || isInvincible) return;

        int actualDamage = Mathf.Max(0, damage);
        CurrentHP = Mathf.Max(0, CurrentHP - actualDamage);
        HPChanged?.Invoke(CurrentHP, maxHP);

        float force = knockbackForce > 0f ? knockbackForce : defaultKnockbackForce;
        playerController.ApplyKnockback(knockbackDirection, force);

        if (CurrentHP <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibleRoutine());
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        CurrentHP = Mathf.Min(maxHP, CurrentHP + Mathf.Max(0, amount));
        HPChanged?.Invoke(CurrentHP, maxHP);
    }

    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        float timer = 0f;
        while (timer < invincibleDuration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isInvincible = false;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        PlayerDied?.Invoke();
    }
}
