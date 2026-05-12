using UnityEngine;

/// <summary>
/// Fキー/□ボタンで近くの IInteractable2D を調べるスクリプトです。
/// 宝箱などの「触れるだけではなく、調べて開くもの」に使います。
/// </summary>
public class PlayerInteractor2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform interactOrigin;

    [Header("Interact Settings")]
    [SerializeField] private float interactRange = 0.9f;
    [SerializeField] private LayerMask interactableLayer;

    private void Awake()
    {
        if (inputReader == null) inputReader = GetComponent<InputReader>();
        if (interactOrigin == null) interactOrigin = transform;
    }

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.InteractPressed += TryInteract;
        }
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.InteractPressed -= TryInteract;
        }
    }

    private void TryInteract()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(interactOrigin.position, interactRange, interactableLayer);
        IInteractable2D nearest = null;
        float nearestDistanceSqr = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            IInteractable2D interactable = hit.GetComponentInParent<IInteractable2D>();
            if (interactable == null) continue;

            float distanceSqr = ((Vector2)hit.transform.position - (Vector2)transform.position).sqrMagnitude;
            if (distanceSqr < nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearest = interactable;
            }
        }

        nearest?.Interact(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Transform origin = interactOrigin != null ? interactOrigin : transform;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(origin.position, interactRange);
    }
}
