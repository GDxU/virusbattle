using UnityEngine;
using System.Collections;

public class CardModel : Model
{
    public Type type;
    public int id;
    public string name;
    public int skill;

    public int health;
    public int attack;
    public int defend;

    public string describe;

    // 怪物卡
    public CardModel(int id, string name, int health, int attack, int defend, int skill, string describe)
    {
        this.type = Type.Monster;
        this.id = id;
        this.name = name;
        this.health = health;
        this.attack = attack;
        this.defend = defend;
        this.skill = skill;
        this.describe = describe;
    }

    // 陷阱卡
    public CardModel(int id, string name, int skill, string describe)
    {
        this.type = Type.Trap;
        this.id = id;
        this.name = name;
        this.health = 0;
        this.attack = 0;
        this.defend = 0;
        this.skill = skill;
        this.describe = describe;
    }

    // 装备卡
    public CardModel(int id, string name, int health, int attack, int defend)
    {
        this.type = Type.Equip;
        this.id = id;
        this.name = name;
        this.health = health;
        this.attack = attack;
        this.defend = defend;
        this.skill = 0;
        this.describe = "";
    }

    public enum Type
    {
        Monster,
        Trap,
        Equip
    }
}
