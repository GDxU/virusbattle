using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CardBattle : MonoBehaviour
{
    private GameObject selfHand;
    private GameObject enemyHand;

    public static Player self = new Player();
    public static Player enemy = new Player();

    // 当前为哪方的回合
    private Round current;

    // 回合数
    //private int round = 0;

    // 召唤次数
    private int callCount = 1;

    public static bool running = false;

    private int clickCount = 0;
    private float currentTime = 0;

    // 怪物长按
    public static bool longDown = false;
    public static Monster longDownMonster = null;

    public static bool down = false;
    public static bool enter = false;
    public static CardView downView = null;
    
    public static Monster downMonster = null;

	private static bool attacking = false;
	private static Vector3 attackPosition = Vector3.zero;
	private static Monster attackMonster = null;
	private static Monster hurtMonster = null;

	public static Text Self_Health;
	public static Image Self_Blood;
	public static Text Self_DeckCount;
	public static Text Enemy_Health;
	public static Image Enemy_Blood;
	public static Text Enemy_DeckCount;

    void Awake()
    {
        EventManager.AddListener(Event.ShowCard, ShowCard);
        EventManager.AddListener(Event.CardPointer, CardPointer);
        EventManager.AddListener(Event.CardDown, CardDown);

        selfHand = transform.FindChild("SelfHand").gameObject;
        enemyHand = transform.FindChild("EnemyHand").gameObject;
		Self_Health = transform.FindChild ("PlayerData/Self/Health").GetComponent<Text>();
		Self_Blood = transform.FindChild ("PlayerData/Self/Blood").GetComponent<Image>();
		Self_DeckCount = transform.FindChild ("PlayerData/Self/DeckCount").GetComponent<Text>();

		Enemy_Health = transform.FindChild ("PlayerData/Enemy/Health").GetComponent<Text>();
		Enemy_Blood = transform.FindChild ("PlayerData/Enemy/Blood").GetComponent<Image>();
		Enemy_DeckCount = transform.FindChild ("PlayerData/Enemy/DeckCount").GetComponent<Text>();
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(Event.ShowCard, ShowCard);
        EventManager.RemoveListener(Event.CardPointer, CardPointer);
        EventManager.RemoveListener(Event.CardDown, CardDown);
    }

	void Start()
    {
        StartCoroutine(StartGame());
    }
	
	void Update()
    {
		// 刷新主角信息
		Refresh ();

        // 连击介绍卡牌信息
        if (clickCount > 0)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
        }
        
        if (clickCount != 0 && currentTime > 0.5f)
        {
            currentTime = 0;
            clickCount = 0;
        }

        if (longDown)
        {
            longDown = false;

            if (longDownMonster.IsMine(current) && longDownMonster.canChangeState)
            {
                HideLine();

                ButtonCancelChange(true);
            }
            else
            {
                longDownMonster = null;
            }

            return;
        }

		// 未处于攻击状态
		if (down && !attacking)
        {
            Vector3 from = Vector3.zero;

            if (downMonster != null)
            {
                if (downMonster.IsMine(current) && downMonster.canAttack)
                {
                    from = new Vector3(downMonster.transform.localPosition.x * (current == Round.Self ? 0.088f : -0.088f), current == Round.Self ? -10f : 10f, 80);
                }
                
            }
            else
            {
                downView.transform.DOComplete();
                from = new Vector3(downView.transform.localPosition.x * (current == Round.Self ? 0.088f : -0.088f), downView.transform.localPosition.y * (current == Round.Self ? -1.5f : 1.5f), 80);
            }

            ShowLine(from);
        }
    }

	private void Refresh()
	{
		Self_Health.text = self.health.ToString();
		Self_Blood.fillAmount = (float)self.health / (float)15;
		Self_DeckCount.text = self.deckCount.ToString();

		Enemy_Health.text = enemy.health.ToString();
		Enemy_Blood.fillAmount = (float)enemy.health / (float)15;
		Enemy_DeckCount.text = enemy.deckCount.ToString();
	}

    public void ShowLine(Vector3 from)
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 80));

        LineRenderer line = GameObject.Find("Line").GetComponent<LineRenderer>();

        line.SetPosition(0, from);
        line.SetPosition(1, target);

        GameObject arrow = line.transform.FindChild("Arrow").gameObject;
        arrow.SetActive(true);
        arrow.transform.position = target;
        arrow.transform.LookAt(from);
    }

    public void HideLine()
    {
        LineRenderer line = GameObject.Find("Line").GetComponent<LineRenderer>();
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);

        GameObject arrow = line.transform.FindChild("Arrow").gameObject;
        arrow.SetActive(false);
        arrow.transform.position = Vector3.zero;
        arrow.transform.rotation = new Quaternion();
    }

    IEnumerator StartGame()
    {
        self = new Player();
        enemy = new Player();

        self.Init(CardSet.Group.Benefit, true);
        enemy.Init(CardSet.Group.Harm, false);

        for (int i = 0; i < 3; i++)
        {
            self.DrawACard(true);
            enemy.DrawACard(true);
        }

        //判断先手
        if (Random.Range(0, 2) == 0)
        {
            self.DrawACard(true);
            current = Round.Self;
        }
        else
        {
            enemy.DrawACard(true);
            current = Round.Enemy;
        }

        self.HandSort(1f);
        enemy.HandSort(1f);

        yield return new WaitForSeconds(2f);

        if (current == Round.Self)
        {
            EventManager.PostEvent(TipSystem.DataType.ShowTip, "我方先手", true);
            BattleDisplay(enemyHand, false);
            self.HandDisplay(true);
            enemy.HandDisplay(false);
        }
        else
        {
            EventManager.PostEvent(TipSystem.DataType.ShowTip, "我方先手", false);
            BattleDisplay(selfHand, false);
            enemy.HandDisplay(true);
            self.HandDisplay(false);
        }

        running = true;
        yield return 0;
    }

    public void ShowCard(object[] param)
    {
        if (running)
        {
            clickCount++;

            if (clickCount == 2)
            {
                CardView view = param[0] as CardView;
                CardView clone = GameObject.Instantiate(view);
                Transform cardShow = transform.parent.FindChild("CardShow");
                cardShow.gameObject.SetActive(true);
                clone.transform.SetParent(cardShow);
                clone.transform.DOLocalMove(new Vector3(0, 0, -350), 0.5f);
                //clone.transform.localPosition = Vector3.zero;
                clone.transform.rotation = new Quaternion();

                if (current == Round.Enemy)
                {
                    clone.transform.Rotate(new Vector3(0, 0, 180));
                }
                clone.transform.localScale = new Vector3(1f, 1f);
                clone.gameObject.AddComponent<CardHide>();
                GameObject.Destroy(clone);
            }
        }
    }

    public void CardPointer(object[] param)
    {
        CardView view = (CardView)param[0];
        bool _enter = (bool)param[1];
        view.transform.DOComplete();

        if (_enter)
        {
            view.transform.DOLocalMoveY(20f, 0.5f);
        }
        else
        {
            if (current == Round.Self)
            {
                self.HandSort();
                //view.transform.position -= new Vector3(0, 0.2f, 0);
            }
            else
            {
                enemy.HandSort();
                //view.transform.position += new Vector3(0, 0.2f, 0);
            }
        }
    }

    public void CardDown(object[] param)
    {
        downView = (CardView)param[0];

        if (!enter && downView != null)
        {
            CardPointer(new object[] { downView, true });
        }
    }
    
    public void OnMouseUp()
    {
		// 处于攻击状态
		if (attacking) {
			return;
		}

        if (downMonster != null)
        {
            downMonster.startLongDown = false;
        }

        if (down)
        {
            //Debug.Log(downView.Name.text + " " + downView.handId);
            enter = false;

            if (!enter)
            {
                if (current == Round.Self)
                {
                    CallMonster(self);
                    self.HandSort();
                }
                else
                {
                    CallMonster(enemy);
                    enemy.HandSort();
                }
            }

            down = false;

            HideLine();
        }
        /*if (!enter && downView != null)
        {
            CardPointer(new object[] { downView, false });
        }

        downView = null;*/
    }

    public void ShowStateSelect()
    {
        GameObject stateSelect = transform.parent.FindChild("StateSelect").gameObject;
        stateSelect.SetActive(true);
        stateSelect.transform.localRotation = current == Round.Self ? new Quaternion() : Quaternion.Euler(0, 0, 180);
    }

    private void CallMonster(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            if (rayHit.collider.name == "BattleRage")
            {
                if (downMonster == null)
                {
                    if (downView.type == CardModel.Type.Monster)
                    {
                        if (callCount > 0)
                        {
                            ShowStateSelect();
                        }
                        else
                        {
                            EventManager.PostEvent(TipSystem.DataType.ShowTip, "不可召唤怪物，已到本回合抽取召唤上限", current == Round.Self ? true : false);
                        }
                    }
                }
            }
            else if (rayHit.collider.name == "Monster(Clone)")
            {
                if (downMonster != null)
                {
                    if (downMonster.IsMine(current) && downMonster.canAttack)
                    {
                        Monster monster = rayHit.collider.GetComponent<Monster>();

                        if (((monster.isSelf && current == Round.Enemy) || (!monster.isSelf && current == Round.Self)) && !attacking)
                        {
                            attacking = true;
                            hurtMonster = monster;
                            attackMonster = downMonster;
                            attackPosition = downMonster.origin.transform.position;

                            downMonster.origin.transform.DOMove(monster.transform.position - new Vector3(0, monster.isSelf ? -15f : 15f), 1f).OnComplete(()=> {
                                // 播放攻击动画 **** 未完待续 ****
								Animator anim = attackMonster.origin.transform.FindChild("Monster").GetComponent<Animator>();
								anim.SetBool("Attack", true);
                            });

                            downMonster.SetState(false, false);
                        }
                        
                    }

                }
                else
                {
                    if (downView.type == CardModel.Type.Equip)
                    {
                        Monster monster = rayHit.collider.GetComponent<Monster>();

                        if ((monster.isSelf && current == Round.Self) || (!monster.isSelf && current == Round.Enemy))
                        {
                            if (monster.equip.Equals(new Monster.MonsterEquip()))
                            {
								CardBattle.PlayAudio ("装备");

                                monster.AddEquip(int.Parse(downView.Health.text), int.Parse(downView.Attack.text), int.Parse(downView.Defend.text));

                                if (monster.isSelf)
                                {
                                    self.RemoveEquip(downView.handId);
                                }
                                else
                                {
                                    enemy.RemoveEquip(downView.handId);
                                }
                                
                                monster.UpdateCard();
                            }
                            else
                            {
                                EventManager.PostEvent(TipSystem.DataType.ShowTip, "当前怪物已有装备", current == Round.Self ? true : false);
                            }
                        }
                    }
                }
            }
        }
        
        downMonster = null;
    }

	// ****************************************
	public static void AnimatorAttack()
	{
		CardBattle.PlayAudio ("受伤");

		// 攻击动画完成
		Animator anim = attackMonster.origin.transform.FindChild("Monster").GetComponent<Animator>();
		anim.SetBool("Attack", false);

		// **** 受伤计算 ****
		// 被攻击者
		if (hurtMonster.state == Monster.State.Attack)
		{
			
			hurtMonster.SetHealth(hurtMonster.GetHealth() - attackMonster.GetValue());
		}
		else
		{
			int value = hurtMonster.GetValue() - attackMonster.GetValue();
			if (value >= 0)
			{
				hurtMonster.SetValue(Monster.State.Defend, value);
			}
			else
			{
				hurtMonster.SetValue(Monster.State.Defend, 0);
				hurtMonster.SetHealth(hurtMonster.GetHealth() + value);
			}

			// 被攻击者切换防御状态到攻击状态
			hurtMonster.state = Monster.State.Attack;
			hurtMonster.SetValue(hurtMonster.GetValue());
		}

		// 被攻击者更新卡牌信息
		hurtMonster.UpdateCard();
		// **** 受伤计算 ****

		// 攻击动画播放完毕攻击的怪物返回原位
		attackMonster.origin.transform.DOMove(attackPosition, 1f).OnComplete(()=> {
			attacking = false;
			attackPosition = Vector3.zero;
			attackMonster = null;
			hurtMonster = null;
		});
	}

    public void CallMonster(Player player, Monster.State state)
    {
		CardBattle.PlayAudio ("召唤");

        player.CallMonster(downView.handId, state);

        if (current == Round.Self)
        {
            self.BattleSort(new Vector3(200f, 0), true);
            enemy.BattleSort(new Vector3(200f, 0), true);
        }
        else
        {
            self.BattleSort(new Vector3(200f, 0), false);
            enemy.BattleSort(new Vector3(200f, 0), false);
        }

        player.HandSort();
        // 当前回合召唤次数减1
        --callCount;
    }

    public void ButtonCallAttackMonster()
    {
        ButtonCancelSelect();
        CallMonster(current == Round.Self ? self : enemy, Monster.State.Attack);
    }

    public void ButtonCallDefendMonster()
    {
        ButtonCancelSelect();
        CallMonster(current == Round.Self ? self : enemy, Monster.State.Defend);
    }

    public void ButtonCancelSelect()
    {
        GameObject stateSelect = transform.parent.FindChild("StateSelect").gameObject;
        stateSelect.SetActive(false);
    }

    public void ButtonChangeState()
    {
        longDownMonster.state = longDownMonster.state == Monster.State.Attack ? Monster.State.Defend : Monster.State.Attack;
        longDownMonster.SetValue(longDownMonster.GetValue());
        longDownMonster.SetState(false, false);

        longDownMonster = null;
        ButtonCancelChange(false);
    }

    public void ButtonCancelChange(bool active = false)
    {
        GameObject stateSelect = transform.parent.FindChild("StateChange").gameObject;
        stateSelect.SetActive(active);
        stateSelect.transform.rotation = current == Round.Self ? new Quaternion() : Quaternion.Euler(0, 0, 180);
    }

    private void BattleDisplay(GameObject battle, bool display = true)
    {
        Transform ButtonTransform = battle.transform.FindChild("Button");
        Button ButtonEnd = battle.transform.FindChild("Button/ButtonEnd").GetComponent<Button>();
        ButtonEnd.interactable = display;

        if (display)
        {
            ButtonTransform.DOLocalMoveX(0, 1);
        }
        else
        {
            ButtonTransform.DOLocalMoveX(300, 1);
        }
        
    }

    public void ButtonEndRound()
    {
        if (running)
        {
            if (current == Round.Self)
            {
                BattleDisplay(enemyHand, true);
                BattleDisplay(selfHand, false);
                self.HandDisplay(false);
                enemy.HandDisplay(true);

                self.BattleSort(new Vector3(200f, 0), false);
                enemy.BattleSort(new Vector3(200f, 0), false);

                enemy.ResetState();
                self.HideState();

                enemy.DrawACard();
                enemy.HandSort();

				int count = self.canAttackCount () - enemy.BattleCount ();
				if (count > 0)
				{
					enemy.health -= count;
				}

                current = Round.Enemy;
                
            }
            else
            {
                BattleDisplay(enemyHand, false);
                BattleDisplay(selfHand, true);
                self.HandDisplay(true);
                enemy.HandDisplay(false);

                self.BattleSort(new Vector3(200f, 0), true);
                enemy.BattleSort(new Vector3(200f, 0), true);

                self.ResetState();
                enemy.HideState();

                self.DrawACard();
                self.HandSort();

				int count = enemy.canAttackCount () - self.BattleCount ();
				if (count > 0)
				{
					self.health -= count;
				}


                current = Round.Self;
                
            }

            // 召唤次数为1
            callCount = 1;
			CardBattle.PlayAudio ("结束");
        }

		// **** 胜利条件 ****
		if (self.health <= 0) {
			GameObject winObj = transform.parent.FindChild ("Win").gameObject;
			winObj.transform.localRotation = Quaternion.Euler (0, 0, -180);
			winObj.SetActive (true);
		} else if (enemy.health <= 0) {
			GameObject winObj = transform.parent.FindChild ("Win").gameObject;
			winObj.transform.localRotation = Quaternion.Euler (0, 0, 0);
			winObj.SetActive (true);
		} else {
			EventManager.PostEvent(TipSystem.DataType.ShowTip, "我方回合", current == Round.Self ? true : false);
		}
		// **** 胜利条件 ****
    }

	public void ButtonReturn()
	{
		GameSystem.StartGame ();
	}

	public static void PlayAudio(string name)
	{
		AudioSource source = GameObject.Find ("CardBattle").GetComponent<AudioSource> ();
		source.clip = Resources.Load<AudioClip> ("Sounds/" + name);
		source.loop = false;
		source.Play ();
	}

    public enum Event
    {
        ShowCard,
        EndRound,
        CardPointer,
        CardDown
    }

    public enum Round
    {
        Self,
        Enemy
    }
}
