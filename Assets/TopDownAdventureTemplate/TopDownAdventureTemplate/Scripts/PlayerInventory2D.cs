using System;
using UnityEngine;

/// <summary>
/// 鍵とコインを管理します。
/// 鍵を「持っている/持っていない」だけにしたい場合は maxKeys を 1 にしてください。
/// </summary>
public class PlayerInventory2D : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] private int maxKeys = 99;

    public int KeyCount { get; private set; }
    public int CoinCount { get; private set; }

    public event Action<int> KeyCountChanged;
    public event Action<int> CoinCountChanged;

    private void Start()
    {
        KeyCountChanged?.Invoke(KeyCount);
        CoinCountChanged?.Invoke(CoinCount);
    }

    public void AddKey(int amount = 1)
    {
        KeyCount = Mathf.Clamp(KeyCount + Mathf.Max(1, amount), 0, maxKeys);
        KeyCountChanged?.Invoke(KeyCount);
    }

    public bool UseKey(int amount = 1)
    {
        int required = Mathf.Max(1, amount);
        if (KeyCount < required) return false;

        KeyCount -= required;
        KeyCountChanged?.Invoke(KeyCount);
        return true;
    }

    public void AddCoins(int amount)
    {
        CoinCount += Mathf.Max(0, amount);
        CoinCountChanged?.Invoke(CoinCount);
    }
}
