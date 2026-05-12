using UnityEngine;

public enum ChestRewardType
{
    Key,
    Coin,
    KeyAndCoin
}

/// <summary>
/// Fキー/□ボタンで開けられる宝箱です。
/// 鍵やコインを入手できます。アイテム所持システムは持たせず、数値だけを増やします。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class TreasureChest2D : MonoBehaviour, IInteractable2D
{
    [Header("Reward")]
    [SerializeField] private ChestRewardType rewardType = ChestRewardType.Coin;
    [SerializeField] private int keyReward = 1;
    [SerializeField] private int coinReward = 10;

    [Header("Visual / Sound")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openedSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource openSound;

    private bool opened;

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();

        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
    }

    public void Interact(GameObject interactor)
    {
        if (opened) return;

        PlayerInventory2D inventory = interactor.GetComponent<PlayerInventory2D>();
        if (inventory == null) return;

        opened = true;

        if (rewardType == ChestRewardType.Key || rewardType == ChestRewardType.KeyAndCoin)
        {
            inventory.AddKey(keyReward);
        }

        if (rewardType == ChestRewardType.Coin || rewardType == ChestRewardType.KeyAndCoin)
        {
            inventory.AddCoins(coinReward);
        }

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        if (spriteRenderer != null && openedSprite != null)
        {
            spriteRenderer.sprite = openedSprite;
        }

        if (openSound != null)
        {
            openSound.Play();
        }
    }

    public string GetInteractMessage()
    {
        return opened ? "空の宝箱" : "宝箱を開ける";
    }
}
