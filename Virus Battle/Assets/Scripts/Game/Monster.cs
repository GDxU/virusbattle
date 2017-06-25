using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public Slider Health { get; set; }
    public Text Text { get; set; }
    public Text Value { get; set; }
    public Image _State { get; set; }

    public State state = State.Attack;
    public bool isSelf = true;
    public CardProto proto = null;
    public GameObject origin = null;

    public MonsterModel model = new MonsterModel();

    // 怪物装备
    public MonsterEquip equip = new MonsterEquip();

    // 可否攻击
    public bool canAttack = false;

    // 可以改变状态
    public bool canChangeState = false;

    private float currentTime = 0;
    public bool startLongDown = false;

    void Awake()
    {
        Health = transform.FindChild("Health").GetComponent<Slider>();
        Text = transform.FindChild("Text/Attack").GetComponent<Text>();
        Value = transform.FindChild("Text/Value").GetComponent<Text>();
        _State = transform.FindChild("State").GetComponent<Image>();

    }

    void Update()
    {
        // 死亡
        if (model.health <= 0)
        {
            if (isSelf)
            {
                CardBattle.self.DropGrave(this);
                CardBattle.self.BattleSort(new Vector3(200f, 0), false);
            }
            else
            {
                CardBattle.enemy.DropGrave(this);
                CardBattle.enemy.BattleSort(new Vector3(200f, 0), true);
            }
        }

        if (startLongDown && canChangeState)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 1.5f)
            {
                CardBattle.down = false;
                CardBattle.longDown = true;
                CardBattle.longDownMonster = this;

                currentTime = 0;
                startLongDown = false;
            }
        }
    }

    void OnMouseDown()
    {
        startLongDown = true;

        CardBattle.down = true;
        CardBattle.downMonster = this;
    }

    void OnMouseExit()
    {
        currentTime = 0;
        startLongDown = false;
    }

    public bool IsMine(CardBattle.Round round)
    {
        if (round == CardBattle.Round.Enemy)
        {
            return isSelf ? false : true;
        }
        else
        {
            return isSelf ? true : false;
        }
    }

    // 怪物状态
    public enum State
    {
        Attack,
        Defend
    }

    public void HideState()
    {
        _State.color = new Color(0, 0, 0);
    }

    public void UpdateMonster()
    {
        Health.value = (float)model.health / (float)(proto.GetHealth() + equip.health);

        if (state == Monster.State.Attack)
        {
            Value.text = (proto.GetAttack() + equip.attack).ToString();
        }
        else
        {
            Value.text = model.value.ToString();
        }

        UpdateCard();
    }

    public void UpdateCard()
    {
        proto.View.Health.text = model.health.ToString();
        proto.View.Attack.text = (proto.GetAttack() + equip.attack).ToString();
        proto.View.Defend.text = model.value.ToString();
    }

    public void AddEquip(int health, int attack, int defend)
    {
        equip.health = health;
        equip.attack = attack;
        equip.defend = defend;

        model.health += health;
        model.value += defend;

        UpdateMonster();
    }

    public void SetState(bool canAttack, bool canChangeState)
    {
        this.canAttack = canAttack;
        this.canChangeState = canChangeState;

        if (canAttack)
        {
            _State.color = new Color(1, 0, 0);
        }
        else
        {
            if (canChangeState)
            {
                _State.color = new Color(0, 1, 0);
            }
            else
            {
                _State.color = new Color(0, 0, 0);
            }
        }
    }

    public void SetHealth(int health)
    {
        model.health = health;
        Health.value = (float)health / (float)(proto.GetHealth() + equip.health);
    }

    public void SetValue(State state, int value)
    {
        if (state == Monster.State.Attack)
        {
            Text.text = "攻击";
        }
        else
        {
            Text.text = "防御";
            model.value = value;
        }

        Value.text = value.ToString();
    }

    public void SetValue(int value)
    {
        SetValue(state, value);
    }

    public int GetHealth()
    {
        return model.health;
    }

    public int GetValue()
    {
        return state == Monster.State.Attack ? proto.GetAttack() + equip.attack : model.value;
    }

    public struct MonsterModel
    {
        public int health;
        public int value;
    }

    public struct MonsterEquip
    {
        public int health;
        public int attack;
        public int defend;
    }
}
