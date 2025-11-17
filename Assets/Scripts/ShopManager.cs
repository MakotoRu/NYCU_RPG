using UnityEngine;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("UI 設定")]
    public GameObject shopPanel;
    public Transform itemContainer;
    public GameObject itemPrefab; // 商品 prefab

    [Header("動畫設定")]
    public float animationDuration = 0.5f;
    public float itemDelay = 0.15f;

    private ShopItem[] spawnedItems;

    public bool IsShopOpen = false;

    void Awake()
    {
        Instance = this;
        shopPanel.SetActive(false);
    }
    
    public void ToggleShop(ShopItemData[] itemsData)
    {
        if (IsShopOpen)
            CloseShop();
        else
            OpenShop(itemsData);
    }

    public void OpenShop(ShopItemData[] itemsData)
    {
        IsShopOpen = true;
        shopPanel.SetActive(true);
        
        UIBackgroundFader.Instance.FadeIn();
        
        foreach (Transform child in itemContainer)
            Destroy(child.gameObject);
        
        spawnedItems = new ShopItem[itemsData.Length];
        
        float spacing = 400f;
        float totalWidth = (itemsData.Length - 1) * spacing;

        for (int i = 0; i < itemsData.Length; i++)
        {
            GameObject go = Instantiate(itemPrefab, itemContainer);
            ShopItem shopItem = go.GetComponent<ShopItem>();
            spawnedItems[i] = shopItem;

            shopItem.Setup(itemsData[i]);

            RectTransform rt = go.GetComponent<RectTransform>();
            
            float x = -totalWidth / 2 + i * spacing;
            Vector2 targetPos = new Vector2(x, 0f);
            
            rt.anchoredPosition = new Vector2(x, targetPos.y - 1000f);
            
            rt.DOAnchorPos(targetPos, animationDuration)
                .SetEase(Ease.OutBack)
                .SetDelay(i * itemDelay);
        }
    }

    public void CloseShop()
    {
        if (spawnedItems == null || spawnedItems.Length == 0)
        {
            shopPanel.SetActive(false);
            return;
        }
        
        UIBackgroundFader.Instance.FadeOut();
        
        int finishedCount = 0;
        int total = spawnedItems.Length;

        for (int i = 0; i < spawnedItems.Length; i++)
        {
            RectTransform rt = spawnedItems[i].GetComponent<RectTransform>();
            Vector2 targetPos = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - 1000f);
            
            rt.DOAnchorPos(targetPos, animationDuration)
                .SetEase(Ease.InBack)
                .SetDelay(i * itemDelay)
                .OnComplete(() =>
                {
                    finishedCount++;
                    if (finishedCount >= total)
                    {
                        shopPanel.SetActive(false);
                        IsShopOpen = false;
                    }
                });
        }
    }

}