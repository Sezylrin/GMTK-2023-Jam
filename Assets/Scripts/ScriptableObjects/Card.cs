using UnityEngine;

public enum CardType
{
    Powerup,
    Weapon
}

[CreateAssetMenu(fileName ="Card", menuName="Cards")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public CardType type;

    public Sprite sprite;
}
