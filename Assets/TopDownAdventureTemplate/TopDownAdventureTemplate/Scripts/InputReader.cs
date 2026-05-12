using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Unity Input System の入力をゲーム用の値・イベントに変換するスクリプトです。
/// PlayerController2D や PlayerCombat2D が直接キー名を知らなくて済むようにします。
/// </summary>
public class InputReader : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference interactAction;

    public Vector2 Move { get; private set; }

    public event Action AttackPressed;
    public event Action InteractPressed;

    private void OnEnable()
    {
        RegisterMoveAction();
        RegisterButtonAction(attackAction, OnAttackPerformed);
        RegisterButtonAction(interactAction, OnInteractPerformed);
    }

    private void OnDisable()
    {
        UnregisterMoveAction();
        UnregisterButtonAction(attackAction, OnAttackPerformed);
        UnregisterButtonAction(interactAction, OnInteractPerformed);
        Move = Vector2.zero;
    }

    private void RegisterMoveAction()
    {
        if (moveAction == null || moveAction.action == null) return;

        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;
        moveAction.action.Enable();
    }

    private void UnregisterMoveAction()
    {
        if (moveAction == null || moveAction.action == null) return;

        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
        moveAction.action.Disable();
    }

    private void RegisterButtonAction(InputActionReference actionReference, Action<InputAction.CallbackContext> callback)
    {
        if (actionReference == null || actionReference.action == null) return;

        actionReference.action.performed += callback;
        actionReference.action.Enable();
    }

    private void UnregisterButtonAction(InputActionReference actionReference, Action<InputAction.CallbackContext> callback)
    {
        if (actionReference == null || actionReference.action == null) return;

        actionReference.action.performed -= callback;
        actionReference.action.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        Move = Vector2.zero;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        AttackPressed?.Invoke();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        InteractPressed?.Invoke();
    }
}
