using UnityEngine;
using System.Collections.Generic; 

[RequireComponent(typeof(BoxCollider2D))]
public class ShopInteract : MonoBehaviour, IInteractable
{
    [Header("item data")]
    public ShopItemData[] itemPool; 

    [Header("amount per sell")]
    public int amountToSell = 3;    // 每次要賣幾張

    // *** 新增的 Checkbox 變數 ***
    [Tooltip("can repeat in same roll?")]
    public bool allowDuplicates = true; // 預設為 true，匹配您上一次的要求 (抽後放回)

    public string GetPromptMessage()
    {
        return "按 E 查看隨機商品";
    }

    public void Interact()
    {
        if (itemPool.Length == 0)
        {
            Debug.LogWarning("商店卡池是空的！");
            return;
        }

        int actualAmountToSell = amountToSell;
        if (!allowDuplicates && amountToSell > itemPool.Length)
        {
            actualAmountToSell = itemPool.Length;
            Debug.LogWarning("販賣數量大於卡池總數，將顯示所有卡片且不重複。");
        }
        
        ShopItemData[] selectedItems = new ShopItemData[actualAmountToSell];

        if (allowDuplicates)
        {
            for (int i = 0; i < actualAmountToSell; i++)
            {
                int randomIndex = Random.Range(0, itemPool.Length);
                selectedItems[i] = itemPool[randomIndex];
            }
        }
        else
        {

            List<ShopItemData> tempPool = new List<ShopItemData>(itemPool);
            
            for (int i = 0; i < actualAmountToSell; i++)
            {
                int randomIndex = Random.Range(0, tempPool.Count);
                selectedItems[i] = tempPool[randomIndex];

                tempPool.RemoveAt(randomIndex);
            }
        }

        ShopManager.Instance.ToggleShop(selectedItems);
    }
}