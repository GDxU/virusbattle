using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Player
{
    private List<CardProto> deck = new List<CardProto>();
    private List<CardProto> hand = new List<CardProto>();
    private List<CardProto> grave = new List<CardProto>();
    private List<CardProto> battle = new List<CardProto>();

    private CardSet.Group group;

    private bool isSelf;

    // 卡牌距离
    private float distance = 100f;
    // 奇数缩放
    private float evenScale = 0.6f;
    // 偶数缩放
    private float oddScale = 0.7f;

    // 最大手牌数
    private int maxHand = 10;

    public CardView current = null;

    // 玩家生命值
    public int health = 15;

    // 玩家卡组数量
    public int deckCount = 20;

	public int BattleCount()
	{
		return battle.Count;
	}

	public int canAttackCount()
	{
		int count = 0;

		for (int i = 0; i < battle.Count; i++)
		{
			if (battle [i].View.monster.GetComponent<Monster> ().canAttack)
			{
				count++;
			}
		}
		return count;
	}
    
    public void Init(CardSet.Group group, bool isSelf)
    {
        this.group = group;
        this.isSelf = isSelf;

        for (int i = 0; i < 6; i++)
        {
            deck.Add(CardSet.GetCard(group, CardSet.Level.Normal));

            Debug.Log(group + " " + "Normal " + deck[i].GetName());
        }

        for (int i = 0; i < 4; i++)
        {
            deck.Add(CardSet.GetCard(group, CardSet.Level.Rare));

            Debug.Log(group + " " + "Rare " + deck[6 + i].GetName());
        }

        for (int i = 0; i < 2; i++)
        {
            deck.Add(CardSet.GetCard(group, CardSet.Level.Elite));

            Debug.Log(group + " " + "Elite " + deck[10 + i].GetName());
        }

        // 抽取4张陷阱卡
        /*for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, deck.Count);
            deck.Insert(index, CardSet.GetTrapCard(group));
        }*/

        // 抽取4张装备卡
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, deck.Count);
            deck.Insert(index, CardSet.GetEquipCard(group));
        }
    }



    public void DrawACard(bool normal = false)
    {
		deckCount = deck.Count;
		
        if (deck.Count > 0)
        {
            if (hand.Count < maxHand)
            {
                // 是否抽取普通卡
                CardProto card = normal ? deck[0] : deck[Random.Range(0, deck.Count)];
                Debug.Log(group + " 抽取一张卡片：" + card.GetName());
                
                deck.Remove(card);
                CardView view = card.Create(group);
                view.handId = hand.Count;

                if (isSelf)
                {
                    view.transform.SetParent(GameObject.Find("CardBattle/SelfHand").transform);
                }
                else
                {
                    view.transform.SetParent(GameObject.Find("CardBattle/EnemyHand").transform);
                }

                // 缩放率为0.5
                view.transform.localPosition = Vector3.zero;
                view.transform.localScale = new Vector3(0.5f, 0.5f);
                hand.Add(card);
            }
            else
            {
                EventManager.PostEvent(TipSystem.DataType.ShowTip, "手牌不得超过10张，请出牌后继续", isSelf ? true : false);
                //Debug.Log("手牌不得超过10张，请出牌后继续");
            }
        }
        else
        {
            EventManager.PostEvent(TipSystem.DataType.ShowTip, "卡组没有可抽取的卡牌", isSelf ? true : false);
        }
    }
    
    public void HandSort(float time = 0.5f)
    {
        bool even = hand.Count % 2 == 0;

        for (int i = 0; i < hand.Count; i++)
        {
            // 偶数
            if (even)
            {
                int middle = hand.Count / 2;

                if (i < middle)
                {
                    // + PI * 0.7 是调整中间两个y坐标不为0，其中0.7为弧度的系数 
                    hand[i].View.transform.DOLocalMove(new Vector3((i - middle) * distance + distance * 0.5f, -(i - middle) * (i - middle) * Mathf.PI * evenScale + Mathf.PI * evenScale), time);
                    //hand[i].View.transform.localPosition = new Vector3((i - middle) * distance + distance * 0.5f, -(i - middle) * (i - middle) * Mathf.PI * evenScale + Mathf.PI * evenScale);
                    hand[i].View.transform.rotation = new Quaternion();
                    //hand[i].View.transform.DOLocalRotate(new Vector3(0, 0, -(i - middle) * 3f * evenScale), time);
                    hand[i].View.transform.Rotate(0, 0, -(i - middle) * 3f * evenScale);
                }
                else
                {
                    hand[i].View.transform.DOLocalMove(new Vector3((i - middle + 1) * distance - distance * 0.5f, -(i - middle + 1) * (i - middle + 1) * Mathf.PI * evenScale + Mathf.PI * evenScale), time);
                    //hand[i].View.transform.localPosition = new Vector3((i - middle + 1) * distance - distance * 0.5f, -(i - middle + 1) * (i - middle + 1) * Mathf.PI * evenScale + Mathf.PI * evenScale);
                    hand[i].View.transform.rotation = new Quaternion();
                    //hand[i].View.transform.DOLocalRotate(new Vector3(0, 0, -(i - middle + 1) * 3f * evenScale), time);
                    hand[i].View.transform.Rotate(0, 0, -(i - middle + 1) * 3f * evenScale);
                }
            }
            else
            {
                int middle = hand.Count / 2;

                if (i < middle || i > middle)
                {
                    hand[i].View.transform.DOLocalMove(new Vector3((i - middle) * distance, -Mathf.Abs((i - middle) * (i - middle) * (Mathf.PI * oddScale))), time);
                    //hand[i].View.transform.localPosition = new Vector3((i - middle) * distance, -Mathf.Abs((i - middle) * (i - middle) * (Mathf.PI * oddScale)));
                    hand[i].View.transform.rotation = new Quaternion();
                    //hand[i].View.transform.DOLocalRotate(new Vector3(0, 0, -(i - middle) * 3f * oddScale), time);
                    hand[i].View.transform.Rotate(0, 0, -(i - middle) * 3f * oddScale);

                }
                else if (i == middle)
                {
                    hand[i].View.transform.DOLocalMove(new Vector3(0, 0), time);
                    //hand[i].View.transform.localPosition = new Vector3(0, 0);
                    hand[i].View.transform.rotation = new Quaternion();

                }
            }

            
        }
    }

    public void HandDisplay(bool display = true)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].View.enabled = display;
        }
    }

    // 召唤怪物
    public void CallMonster(int index, Monster.State state)
    {
        if (battle.Count <= 5)
        {
            CardProto proto = hand[index];

            if (proto.View.type == CardModel.Type.Monster)
            {
                if (isSelf)
                {
                    proto.View.transform.SetParent(GameObject.Find("CardBattle/SelfBattle").transform);
                }
                else
                {
                    proto.View.transform.SetParent(GameObject.Find("CardBattle/EnemyBattle").transform);
                }

                proto.View.transform.rotation = new Quaternion();
                proto.View.transform.localScale = new Vector3(0.4f, 0.4f);
                //proto.View.transform.DOLocalMove(Vector3.zero, 0.5f);
                proto.View.enabled = false;
                //GameObject.Destroy(proto.View);

                hand.Remove(proto);

                // 创建怪物模型
                Object origin = Resources.Load("Prefab/Monster");
                GameObject monsterObject = GameObject.Instantiate(origin) as GameObject;

                // 修改怪物模型
				string id = (proto.GetID() / 10 == 0 ? "0" : "") + proto.GetID().ToString();
				origin = Resources.Load("Prefab/Role/" + (isSelf ? "Benefit" : "Harm") + "/" + id);
				if (origin == null) {
					origin = Resources.Load ("Prefab/Role/Benefit/01");
				}
                GameObject monsterOrigin = GameObject.Instantiate(origin, monsterObject.transform) as GameObject;
                monsterOrigin.name = "Monster";
                monsterOrigin.transform.DOLocalMoveZ(-50, 0.5f);

                proto.View.monster = monsterObject;
                //proto.View.monster.SetActive(false);

                Monster monster = proto.View.monster.AddComponent<Monster>();
                monster.proto = proto;
                monster.origin = monsterOrigin;
                // 攻击状态
                monster.isSelf = isSelf;
                monster.state = state;
                monster.SetState(false, false);
                monster.SetHealth(proto.GetHealth());
                monster.state = state;
                monster.model.value = proto.GetDefend();
                monster.SetValue(monster.GetValue());

                if (isSelf)
                {
                    proto.View.monster.transform.SetParent(GameObject.Find("CardBattle/SelfBattle/Monsters").transform);
                }
                else
                {
                    proto.View.monster.transform.SetParent(GameObject.Find("CardBattle/EnemyBattle/Monsters").transform);
                }

                proto.View.monster.transform.rotation = new Quaternion();
                proto.View.monster.transform.localScale = new Vector3(1f, 1f, 1f);

                // 更新卡牌信息
                monster.UpdateCard();

                battle.Add(proto);

                for (int i = index; i < hand.Count; i++)
                {
                    hand[i].View.handId = i;
                }
            }

        }
        else
        {
            EventManager.PostEvent(TipSystem.DataType.ShowTip, "不可召唤，战场怪物数量到达上限", isSelf ? true : false);
        }
    }

    public void RemoveEquip(int index)
    {
        CardProto proto = hand[index];
        if (proto.View.type == CardModel.Type.Equip)
        {
            hand.Remove(proto);
            proto.Destroy();
            grave.Add(proto);

			for (int i = index; i < hand.Count; i++)
			{
				hand[i].View.handId = i;
			}
        }
    }

    public void ResetState()
    {
        for (int i = 0; i < battle.Count; i++)
        {
            Monster monster = battle[i].View.monster.GetComponent<Monster>();
            if (monster.state == Monster.State.Attack)
            {
                monster.SetState(true, true);
            }
            else
            {
                monster.SetState(false, true);
            }
        }
    }

    public void HideState()
    {
        for (int i = 0; i < battle.Count; i++)
        {
            Monster monster = battle[i].View.monster.GetComponent<Monster>();
            monster.HideState();
        }
    }

    public void BattleSort(Vector3 distance, bool self, float time = 1f)
    {
        for (int i = 0; i < battle.Count; i++)
        {
            int number = i + 1;
            
            int max = battle.Count / 2 + 1;

            // 负轴坐标
            Vector3 lossPosition = (max - number) * distance;
            // 正轴坐标
            Vector3 mainPosition = (number - max) * distance;

            Vector3 position = Vector3.zero;

            if (number < max)
            {
                // 负轴
                position -= lossPosition;
            }
            else if (number > max)
            {
                // 正轴
                position += mainPosition;
            }
            
            position = battle.Count % 2 == 0 ? position + distance * 0.5f : position;
            current = battle[i].View;

            current.transform.DOLocalMove(position, time);
            current.transform.DORotate(new Vector3(0, 0, 90), time);
            current.transform.DOScale(new Vector3(0.2f, 0.2f, 1), time).OnComplete(()=> {
                current.monster.transform.localPosition = current.transform.localPosition;
            });

            if (current.monster != null)
            {
                current.monster.transform.DOLocalMove(position, time);
                Transform textTransform = current.monster.transform.FindChild("Text").transform;
                textTransform.DORotate(new Vector3(0, 0, self ? 0 : 180), time);
            }
        }
    }

    public void DropGrave(Monster monster)
    {
        battle.Remove(monster.proto);
        monster.proto.Destroy();
        GameObject.Destroy(monster.gameObject);
        grave.Add(monster.proto);

    }
}
