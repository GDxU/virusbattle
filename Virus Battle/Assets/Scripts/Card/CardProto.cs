using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardProto : Proto<CardModel, CardView>
{
    public CardView View
    {
        get { return this.view; }
    }

    public CardProto(CardModel model) : base(model)
    {
    }

    public int GetID()
    {
        return this.model.id;
    }

    public string GetName()
    {
        return this.model.name;
    }

    public int GetHealth()
    {
        return this.model.health;
    }

    public int GetAttack()
    {
        return this.model.attack;
    }

    public int GetDefend()
    {
        return this.model.defend;
    }

    public int GetSkill()
    {
        return this.model.skill;
    }

    public void SetName(string name)
    {
        this.model.name = name;
        this.view.Name.text = this.model.name;
    }

    public void SetHealth(int health)
    {
        this.model.health = health;
        this.view.Health.text = this.model.health.ToString();
    }

    public void SetAttack(int attack)
    {
        this.model.attack = attack;
        this.view.Attack.text = this.model.attack.ToString();
    }

    public void SetDefend(int defend)
    {
        this.model.defend = defend;
        this.view.Defend.text = this.model.defend.ToString();
    }
    
    public CardView Create(CardSet.Group group)
    {
        Object origin = Resources.Load("Prefab/Card");
        GameObject gameObject = GameObject.Instantiate(origin) as GameObject;
        this.view = gameObject.AddComponent<CardView>();

        this.view.Name.text = this.model.name;
        this.view.Describe.text = this.model.describe;

        string id = (this.model.id / 10 == 0 ? "0" : "") + this.model.id;

        if (this.model.type == CardModel.Type.Monster)
        {
            this.view.type = CardModel.Type.Monster;
            this.view.Health.text = this.model.health.ToString();
            this.view.Attack.text = this.model.attack.ToString();
            this.view.Defend.text = this.model.defend.ToString();

            switch (group)
            {
                case CardSet.Group.Benefit:
                    this.view.Group.text = "益";
                    this.view.Avatar.sprite = Resources.Load<Sprite>("Sprites/Role/Benefit/" + id);
                    break;
                case CardSet.Group.Harm:
                    this.view.Group.text = "害";
                    this.view.Avatar.sprite = Resources.Load<Sprite>("Sprites/Role/Harm/" + id);
                    break;
            }

            switch (this.model.skill)
            {
                case 1:
                    this.view.Skill.text = "技能 / 入侵";
                    break;
                case 2:
                    this.view.Skill.text = "技能 / 拦截";
                    break;
                case 3:
                    this.view.Skill.text = "技能 / 反击";
                    break;
                case 4:
                    this.view.Skill.text = "技能 / 召唤";
                    break;
                case 5:
                    this.view.Skill.text = "技能 / 刺杀";
                    break;
                case 6:
                    this.view.Skill.text = "技能 / 守护";
                    break;
                case 7:
                    this.view.Skill.text = "技能 / 诈尸";
                    break;
                case 8:
                    this.view.Skill.text = "技能 / 增强";
                    break;
                default:
                    this.view.Skill.text = "技能 / 无";
                    break;
            }
        }
        else if (this.model.type == CardModel.Type.Trap)
        {
            this.view.type = CardModel.Type.Trap;
            this.view.Health.text = "";
            this.view.Attack.text = "";
            this.view.Defend.text = "";
            this.view.Skill.text = "陷阱卡";

            this.view.transform.FindChild("Front/TextHealth").GetComponent<Text>().text = "";
            this.view.transform.FindChild("Front/TextAttack").GetComponent<Text>().text = "";
            this.view.transform.FindChild("Front/TextDefend").GetComponent<Text>().text = "";

            switch (group)
            {
                case CardSet.Group.Benefit:
                    this.view.Group.text = "益";
                    //this.view.Avatar.sprite = Resources.Load<Sprite>("Sprites/Role/Benefit/" + id);
                    break;
                case CardSet.Group.Harm:
                    this.view.Group.text = "害";
                    //this.view.Avatar.sprite = Resources.Load<Sprite>("Sprites/Role/Harm/" + id);
                    break;
            }
        }
        else if (this.model.type == CardModel.Type.Equip)
        {
            this.view.type = CardModel.Type.Equip;
            this.view.Health.text = this.model.health.ToString();
            this.view.Attack.text = this.model.attack.ToString();
            this.view.Defend.text = this.model.defend.ToString();
            this.view.Skill.text = "装备卡";

            switch (group)
            {
                case CardSet.Group.Benefit:
                    this.view.Group.text = "益";
                    this.view.Avatar.sprite = Resources.Load<Sprite>("Sprites/Equip/Benefit/" + id);
                    break;
                case CardSet.Group.Harm:
                    this.view.Group.text = "害";
                    this.view.Avatar.sprite = Resources.Load<Sprite>("Sprites/Equip/Harm/" + id);
                    break;
            }
        }


        this.view.gameObject.GetComponent<Button>().onClick.AddListener(OnClick);

        return this.view;
    }

    public void Destroy()
    {
        GameObject.Destroy(this.view.gameObject);
    }

    public void OnClick()
    {
        EventManager.PostEvent(CardBattle.Event.ShowCard, this.view);
    }
}
