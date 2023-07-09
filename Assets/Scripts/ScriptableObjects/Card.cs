using UnityEngine;

public enum CardInfo
{
    AttackSpeedUp,
    Healing,
    AttackDamageUp,
    IncreaseMovespeed,
    SwitchDamageAndHealth,

    Sword,
    Spear,
    Dagger,

    Fireball,
    Iceball,
    MeteorShower,

    Dragon,
    BlackWidow,
    Cyclops,
}

public enum CardType
{
    Powerup,
    Weapon,
    Spell,
    Character,
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
    public CardInfo info;

    public Sprite sprite;
}
