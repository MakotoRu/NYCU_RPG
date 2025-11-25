using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image image;
    public TextMeshProUGUI cost;
    void Start()
    {
        if(card){
            title.text = card.cardName;
            description.text = card.description;
            image.sprite = card.image;
            cost.text = card.cost.ToString();
        }
    }

}
