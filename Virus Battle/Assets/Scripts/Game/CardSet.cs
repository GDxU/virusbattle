using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CardSet
{
    // 怪物卡
    private static Dictionary<Level, List<CardModel>> dic_benefit = new Dictionary<Level, List<CardModel>>();
    private static Dictionary<Level, List<CardModel>> dic_harm = new Dictionary<Level, List<CardModel>>();

    // 陷阱卡
    private static List<CardModel> dic_benefit_trap = new List<CardModel>();
    private static List<CardModel> dic_harm_trap = new List<CardModel>();

    // 装备卡
    private static List<CardModel> dic_benefit_equip = new List<CardModel>();
    private static List<CardModel> dic_harm_equip = new List<CardModel>();

    static CardSet()
    {
        // 怪物卡
        List<CardModel> benefit = new List<CardModel>();
        benefit.Add(new CardModel(1, "大肠杆菌", 2, 1, 2, 0, "有被恶性病毒感染的危险，感染后将对己方进行攻击。"));
        benefit.Add(new CardModel(2, "产黄青霉", 1, 3, 1, 7, "产生大量青霉素，生于自然，可治愈人体疾病。"));
        benefit.Add(new CardModel(3, "放线菌", 1, 3, 2, 0, "产生大量抗生素，治愈人体。"));
        benefit.Add(new CardModel(4, "双歧杆菌", 3, 2, 0, 0, "对人体必不可少，多则人体健康，少则人体衰弱。"));
        benefit.Add(new CardModel(5, "乳酸菌", 3, 0, 1, 0, "活跃于人体肠道内，伴随人体发育成长必不可少,可增强人体寿命。"));
        dic_benefit.Add(Level.Normal, benefit);

        benefit = new List<CardModel>();
        benefit.Add(new CardModel(6, "酵母菌", 4, 1, 1, 1, "可以产生抗生素，但以毒攻毒的结果会对人体造成损伤。"));
        benefit.Add(new CardModel(7, "天然病毒(M1型)", 2, 3, 2, 0, "主要针对与癌细胞的战斗，并且纯天然无公害。"));
        benefit.Add(new CardModel(8,"白细胞", 4, 3, 0, 6, "遍布广泛，靠人多取胜，吞噬有害细菌，起到防御疾病的作用。"));
        dic_benefit.Add(Level.Rare, benefit);

        benefit = new List<CardModel>();
        benefit.Add(new CardModel(9, "吞噬细胞", 4, 2, 4, 4, "唤醒其他免疫细胞，对病原体进行吞噬。"));
        benefit.Add(new CardModel(10, "纳米机器人", 4, 5, 2, 8, "帮助有益菌锁定病原体，并进一步投递药物进行药物治疗。"));
        dic_benefit.Add(Level.Elite, benefit);

        List<CardModel> harm = new List<CardModel>();
        harm.Add(new CardModel(1, "葡萄球菌", 1, 3, 2, 0, "针对白细胞进行吞噬，主要寄居在皮肤内或有关节炎症的敌方。"));
        harm.Add(new CardModel(2, "白色念珠菌", 4, 0, 1, 0, "形成白色物质，造成口腔溃疡等，使伤口不易好转甚至溃烂。"));
        harm.Add(new CardModel(3, "梅毒", 1, 2, 2, 2, "牵一发而动全身，一开始小面积溃烂进而导致全身。"));
        harm.Add(new CardModel(4, "流感病毒", 2, 3, 0, 0, "对于人体的呼吸道造成创伤，引起病毒性感冒。"));
        harm.Add(new CardModel(5, "沙门氏菌", 2, 1, 3, 0, "在肠道内进行潜伏，导致人体上吐下泻，疼痛不已。"));
        dic_harm.Add(Level.Normal, harm);

        harm = new List<CardModel>();
        harm.Add(new CardModel(6, "流感病毒(H7N9型)", 3, 3, 0, 4, "从家禽身上开始蔓延的病毒，人体通过食用感染，造成病毒性感冒和肺炎。"));
        harm.Add(new CardModel(7, "人脑病毒", 3, 3, 1, 6, "人为性病毒，对于脑部攻击有着前所未有的效果。"));
        harm.Add(new CardModel(8, "朊病毒", 1, 3, 4, 0, "针对大脑进行精神上的攻击，造成心理上的崩溃。"));
        dic_harm.Add(Level.Rare, harm);

        harm = new List<CardModel>();
        harm.Add(new CardModel(9, "埃博拉病毒", 3, 3, 3, 5, "擅长潜伏，突然爆发。主要针对内脏进行攻击，一旦潜伏成功后果不堪设想。"));
        harm.Add(new CardModel(10, "纳米病毒", 4, 4, 0, 3, "根据抗体的不同特性可随时改变战略进行针对性攻击。"));
        dic_harm.Add(Level.Elite, harm);

        // 陷阱卡
        dic_benefit_trap.Add(new CardModel(1, "改良疫苗", 1, "1111111"));
        dic_benefit_trap.Add(new CardModel(2, "生物壁垒", 2, "1111111"));
        dic_benefit_trap.Add(new CardModel(3, "基因改造", 3, "1111111"));
        dic_benefit_trap.Add(new CardModel(4, "基因退化", 4, "1111111"));
        dic_benefit_trap.Add(new CardModel(5, "错误转录", 5, "1111111"));

        dic_harm_trap.Add(new CardModel(1, "恶性增值", 1, "1111111"));
        dic_harm_trap.Add(new CardModel(2, "干扰翻译", 2, "1111111"));
        dic_harm_trap.Add(new CardModel(3, "基因改造", 3, "1111111"));
        dic_harm_trap.Add(new CardModel(4, "基因退化", 4, "1111111"));
        dic_harm_trap.Add(new CardModel(5, "错误转录", 5, "1111111"));

        // 装备卡
        dic_benefit_equip.Add(new CardModel(1, "生物溶解", 1, 0, 1));
        dic_benefit_equip.Add(new CardModel(2, "核酸气泡", 0, 0, 2));
        dic_benefit_equip.Add(new CardModel(3, "抗性盔甲", 2, 0, 0));
        dic_benefit_equip.Add(new CardModel(4, "基因锁链", 0, 1, 0));
        dic_benefit_equip.Add(new CardModel(5, "基化卡片", 1, 0, 1));

        dic_harm_equip.Add(new CardModel(1, "突变光圈", 1, 1, 0));
        dic_harm_equip.Add(new CardModel(2, "免疫榴弹", 0, 1, 1));
        dic_harm_equip.Add(new CardModel(3, "抗性盔甲", 2, 0, 0));
        dic_harm_equip.Add(new CardModel(4, "基因锁链", 0, 1, 0));
        dic_harm_equip.Add(new CardModel(5, "基化卡片", 1, 0, 1));
    }

    public static CardProto GetEquipCard(Group group)
    {
        int index = 0;
        CardModel model = null;

        switch (group)
        {
            case Group.Benefit:
                index = Random.Range(0, dic_benefit_equip.Count);
                model = dic_benefit_equip[index];
                return new CardProto(model);
            case Group.Harm:
                index = Random.Range(0, dic_harm_equip.Count);
                model = dic_harm_equip[index];
                return new CardProto(model);
            default:
                return null;
        }
    }

    public static CardProto GetTrapCard(Group group)
    {
        int index = 0;
        CardModel model = null;

        switch (group)
        {
            case Group.Benefit:
                index = Random.Range(0, dic_benefit_trap.Count);
                model = dic_benefit_trap[index];
                return new CardProto(model);
            case Group.Harm:
                index = Random.Range(0, dic_harm_trap.Count);
                model = dic_harm_trap[index];
                return new CardProto(model);
            default:
                return null;
        }
    }

    public static CardProto GetCard(Group group, Level level)
    {
        switch (group)
        {
            case Group.Benefit:
                return GetCardProto(dic_benefit, level);
            case Group.Harm:
                return GetCardProto(dic_harm, level);
            default:
                return null;
        }
    }

    private static CardProto GetCardProto(Dictionary<Level, List<CardModel>> list, Level level)
    {
        int index = Random.Range(0, list[level].Count);
        CardModel model = list[level][index];
        return new CardProto(model);
    }

    public enum Group
    {
        Benefit,
        Harm
    }

    public enum Level
    {
        Normal,
        Rare,
        Elite
    }
}
