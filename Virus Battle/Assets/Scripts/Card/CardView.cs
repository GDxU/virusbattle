using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CardView : View, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Text Name { get; set; }
    public Text Health { get; set; }
    public Text Attack { get; set; }
    public Text Defend { get; set; }
    public Text Group { get; set; }
    public Text Describe { get; set; }
    public Text Skill { get; set; }

    public Image Avatar { get; set; }
    
    public int handId = 0;

    // 召唤后的怪物
    public GameObject monster = null;

    // 卡牌类型
    public CardModel.Type type = CardModel.Type.Monster;
    
    void Awake()
    {
        Name = transform.FindChild("Front/Name").GetComponent<Text>();
        Health = transform.FindChild("Front/Health").GetComponent<Text>();
        Attack = transform.FindChild("Front/Attack").GetComponent<Text>();
        Defend = transform.FindChild("Front/Defend").GetComponent<Text>();
        Group = transform.FindChild("Front/ImageGroup/Text").GetComponent<Text>();
        Describe = transform.FindChild("Front/Describe").GetComponent<Text>();
        Skill = transform.FindChild("Front/Skill").GetComponent<Text>();

        Avatar = transform.FindChild("Front/ImageCard").GetComponent<Image>();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CardBattle.running && CardBattle.enter)
        {
            if (!CardBattle.down)
            {
                EventManager.PostEvent(CardBattle.Event.CardPointer, this, false);
            }
            
            CardBattle.enter = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardBattle.running && !CardBattle.enter)
        {
            if (!CardBattle.down)
            {
                EventManager.PostEvent(CardBattle.Event.CardPointer, this, true);
            }
            
            CardBattle.enter = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CardBattle.running && !CardBattle.down)
        {
            EventManager.PostEvent(CardBattle.Event.CardDown, this);
            CardBattle.down = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
