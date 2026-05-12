using UnityEngine;

/// <summary>
/// Fキー/□ボタンで調べられるものが実装するインターフェースです。
/// 宝箱、看板、NPCなどに使えます。
/// </summary>
public interface IInteractable2D
{
    void Interact(GameObject interactor);
    string GetInteractMessage();
}
