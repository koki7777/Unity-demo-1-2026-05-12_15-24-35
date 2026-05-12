using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HPバー、鍵数、コイン数、攻撃力、撃破数をUIに表示します。
/// </summary>
public class HUDController2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth2D playerHealth;
    [SerializeField] private PlayerInventory2D playerInventory;
    [SerializeField] private PlayerProgression2D playerProgression;

    [Header("HP UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    [Header("Status UI")]
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defeatedText;

    private void Awake()
    {
        if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth2D>();
        if (playerInventory == null) playerInventory = FindFirstObjectByType<PlayerInventory2D>();
        if (playerProgression == null) playerProgression = FindFirstObjectByType<PlayerProgression2D>();
    }

    private void OnEnable()
    {
        if (playerHealth != null) playerHealth.HPChanged += UpdateHP;
        if (playerInventory != null)
        {
            playerInventory.KeyCountChanged += UpdateKeys;
            playerInventory.CoinCountChanged += UpdateCoins;
        }
        if (playerProgression != null) playerProgression.ProgressionChanged += UpdateProgression;
    }

    private void OnDisable()
    {
        if (playerHealth != null) playerHealth.HPChanged -= UpdateHP;
        if (playerInventory != null)
        {
            playerInventory.KeyCountChanged -= UpdateKeys;
            playerInventory.CoinCountChanged -= UpdateCoins;
        }
        if (playerProgression != null) playerProgression.ProgressionChanged -= UpdateProgression;
    }

    private void Start()
    {
        if (playerHealth != null) UpdateHP(playerHealth.CurrentHP, playerHealth.MaxHP);
        if (playerInventory != null)
        {
            UpdateKeys(playerInventory.KeyCount);
            UpdateCoins(playerInventory.CoinCount);
        }
        if (playerProgression != null)
        {
            UpdateProgression(playerProgression.DefeatedEnemyCount, playerProgression.AttackLevel, playerProgression.CurrentAttack);
        }
    }

    private void UpdateHP(int current, int max)
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = max;
            hpSlider.value = current;
        }

        if (hpText != null)
        {
            hpText.text = $"HP {current}/{max}";
        }
    }

    private void UpdateKeys(int keyCount)
    {
        if (keyText != null)
        {
            keyText.text = $"鍵 × {keyCount}";
        }
    }

    private void UpdateCoins(int coinCount)
    {
        if (coinText != null)
        {
            coinText.text = $"Coin × {coinCount}";
        }
    }

    private void UpdateProgression(int defeatedCount, int level, int currentAttack)
    {
        if (attackText != null)
        {
            attackText.text = $"攻撃力 {currentAttack}";
        }

        if (defeatedText != null)
        {
            defeatedText.text = $"撃破 {defeatedCount}";
        }
    }
}
