using UnityEngine;

/// <summary>
/// ダメージを受けるオブジェクトが実装する共通インターフェースです。
/// プレイヤーの剣、罠、弾など、攻撃側の種類が増えても同じ呼び方でダメージを与えられます。
/// </summary>
public interface IDamageable2D
{
    void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce, GameObject attacker);
}
