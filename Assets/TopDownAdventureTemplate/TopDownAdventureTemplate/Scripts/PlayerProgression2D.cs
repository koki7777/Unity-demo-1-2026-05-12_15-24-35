using System;
using UnityEngine;

/// <summary>
/// 敵撃破数による攻撃力上昇を管理します。
/// 例：10体倒すごとに攻撃力+5。
/// </summary>
public class PlayerProgression2D : MonoBehaviour
{
    [Header("Attack Growth")]
    [SerializeField] private int baseAttack = 10;
    [SerializeField] private int enemiesPerAttackUp = 10;
    [SerializeField] private int attackIncreasePerLevel = 5;

    public int DefeatedEnemyCount { get; private set; }
    public int AttackLevel => enemiesPerAttackUp <= 0 ? 0 : DefeatedEnemyCount / enemiesPerAttackUp;
    public int CurrentAttack => baseAttack + AttackLevel * attackIncreasePerLevel;

    public event Action<int, int, int> ProgressionChanged;

    private void Start()
    {
        ProgressionChanged?.Invoke(DefeatedEnemyCount, AttackLevel, CurrentAttack);
    }

    public void RegisterEnemyDefeated(int count = 1)
    {
        DefeatedEnemyCount += Mathf.Max(1, count);
        ProgressionChanged?.Invoke(DefeatedEnemyCount, AttackLevel, CurrentAttack);
    }
}
