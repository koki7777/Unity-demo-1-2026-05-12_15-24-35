using System.Collections;
using UnityEngine;

/// <summary>
/// 鍵付き扉です。
/// プレイヤーが触れる、またはFキー/□ボタンで調べると、鍵があれば開きます。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LockedDoor2D : MonoBehaviour, IInteractable2D
{
    [Header("Door Settings")]
    [SerializeField] private int requiredKeys = 1;
    [SerializeField] private bool openWhenTouched = true;
    [SerializeField] private float disableDelayAfterOpen = 0.15f;
    [SerializeField] private bool destroyAfterOpen = false;

    [Header("Visual / Sound")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource unlockSound;
    [SerializeField] private AudioSource lockedSound;
    [SerializeField] private float lockedSoundCooldown = 0.5f;

    private bool opened;
    private float nextLockedSoundTime;
    private Collider2D doorCollider;

    private void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!openWhenTouched) return;
        TryOpenFromObject(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!openWhenTouched) return;
        TryOpenFromObject(other.gameObject);
    }

    public void Interact(GameObject interactor)
    {
        TryOpenFromObject(interactor);
    }

    public string GetInteractMessage()
    {
        return opened ? "開いている扉" : "鍵付き扉";
    }

    private void TryOpenFromObject(GameObject obj)
    {
        if (opened) return;
        if (!obj.CompareTag("Player")) return;

        PlayerInventory2D inventory = obj.GetComponent<PlayerInventory2D>();
        if (inventory == null) return;

        if (!inventory.UseKey(requiredKeys))
        {
            PlayLockedSound();
            return;
        }

        Open();
    }

    private void Open()
    {
        opened = true;

        if (unlockSound != null) unlockSound.Play();
        if (animator != null) animator.SetTrigger("Open");

        StartCoroutine(DisableDoorRoutine());
    }

    private IEnumerator DisableDoorRoutine()
    {
        yield return new WaitForSeconds(disableDelayAfterOpen);

        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        if (destroyAfterOpen)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void PlayLockedSound()
    {
        if (lockedSound == null) return;
        if (Time.time < nextLockedSoundTime) return;

        lockedSound.Play();
        nextLockedSoundTime = Time.time + lockedSoundCooldown;
    }
}
