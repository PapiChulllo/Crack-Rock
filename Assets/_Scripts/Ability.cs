using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public string name;
    public int damage;
    public int manaCost;

    public Ability(string name, int damage, int manaCost)
    {
        this.name = name;
        this.damage = damage;
        this.manaCost = manaCost;
    }
}
