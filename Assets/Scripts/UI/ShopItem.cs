using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text priceText;
    public Button buyButton;

    public void Setup(ShopItemData data)
    {
        icon.sprite = data.icon;
        nameText.text = data.itemName;
        descriptionText.text = data.description;
        priceText.text = data.price.ToString() + "$";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            // TODO: 扣錢/增加背包
        });
    }
}