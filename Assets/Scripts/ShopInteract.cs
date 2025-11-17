using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ShopInteract : MonoBehaviour, IInteractable
{
    public ShopItemData[] items;

    public string GetPromptMessage()
    {
        return "按 E 開啟/關閉商店";
    }

    public void Interact()
    {
        ShopManager.Instance.ToggleShop(items);
    }
}