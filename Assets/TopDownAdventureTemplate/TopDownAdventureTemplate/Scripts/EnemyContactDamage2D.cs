using UnityEngine;

/// <summary>
/// 敵がプレイヤーに接触したときにダメージを与えるスクリプトです。
/// プレイヤー側の無敵時間があるため、接触し続けても毎フレームHPが減ることはありません。
/// </summary>
public class EnemyContactDamage2D : MonoBehaviour
{
    [Header("Contact Damage")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private float damageInterval = 0.6f;

    private float nextDamageTime;

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamagePlayer(other.gameObject);
    }

    private void TryDamagePlayer(GameObject target)
    {
        if (Time.time < nextDamageTime) return;
        if (!target.CompareTag("Player")) return;

        PlayerHealth2D playerHealth = target.GetComponent<PlayerHealth2D>();
        if (playerHealth == null) return;

        Vector2 knockbackDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
        playerHealth.TakeDamage(attackDamage, knockbackDirection, knockbackForce, gameObject);
        nextDamageTime = Time.time + damageInterval;
    }
}
