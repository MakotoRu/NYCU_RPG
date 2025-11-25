using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public int id;
    public string cardName;
    public string description;

    public Sprite image;
    public int cost;
}
