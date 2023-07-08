using UnityEngine;

public enum CardType
{
    Powerup,
    Weapon,
    Spell,
}

[CreateAssetMenu(fileName ="Card", menuName="Cards")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public int manaCost;
    public int attack;
    public int health;
    public CardType type;

    public Sprite sprite;
}
