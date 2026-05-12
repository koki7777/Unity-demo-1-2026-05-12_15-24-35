using UnityEngine;

/// <summary>
/// フィールド上に置く鍵アイテムです。
/// Player が触れると鍵を入手して、このオブジェクトは消えます。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class KeyPickup2D : MonoBehaviour
{
    [SerializeField] private int keyAmount = 1;
    [SerializeField] private AudioSource pickupSound;
    [SerializeField] private bool destroyAfterPickup = true;

    private bool pickedUp;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;

        PlayerInventory2D inventory = other.GetComponent<PlayerInventory2D>();
        if (inventory == null) return;

        pickedUp = true;
        inventory.AddKey(keyAmount);

        if (pickupSound != null)
        {
            pickupSound.Play();
        }

        if (destroyAfterPickup)
        {
            Destroy(gameObject);
        }
    }
}
