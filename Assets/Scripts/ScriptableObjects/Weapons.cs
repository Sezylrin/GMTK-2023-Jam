using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons")]
public class Weapons : ScriptableObject
{
    public GameObject weaponObj;
    public float attackDelay;
    public float attackSpeed;
    public int damage;
    public float Ydist;
}
