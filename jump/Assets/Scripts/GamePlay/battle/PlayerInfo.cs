using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfo {
    private const int skillNum = 4;
    private const int itemNum = 5;
    private const int equipNum = 3;

    private RoleType roleType = RoleType.Warrior;
    private int m_lv;
    private int m_exp;
    private int m_maxExp;
    private int m_hp;
    private int m_maxHp;
    private int m_mp;
    private int m_maxMp;
    private int m_atk;
    private int m_gold;

    public List<int> skillList = new List<int>();
    public List<ItemData> itemList = new List<ItemData>();
    public EquipData[] arrEquip = new EquipData[equipNum];

    public int Lv {
        get { return m_lv; }
        set { 
            m_lv = value;
            MaxExp = RefLv.GetNextLvExp(m_lv);
            Send.SendMsg(SendType.LvChange, Lv);
        }
    }

    public int Exp {
        get { return m_exp; }
        set {
            m_exp = value;
            if (m_exp >= MaxExp) {
                Exp -= MaxExp;
                Lv++;
            }

            Send.SendMsg(SendType.ExpChange, Exp, MaxExp);
        }
    }

    public int MaxExp {
        get { return m_maxExp; }
        set { m_maxExp = value; Send.SendMsg(SendType.ExpChange, Exp, MaxExp); }
    }

    public int Hp {
        get { return m_hp; }
        set { m_hp = value; Send.SendMsg(SendType.HpChange, Hp, MaxHp); }
    }

    public int MaxHp {
        get { return m_maxHp; }
        set { m_maxHp = value; Send.SendMsg(SendType.HpChange, Hp, MaxHp); }
    }

    public int Mp {
        get { return m_mp; }
        set { m_mp = value; Send.SendMsg(SendType.MpChange, Mp, MaxMp); }
    }

    public int MaxMp {
        get { return m_maxMp; }
        set { m_maxMp = value; Send.SendMsg(SendType.MpChange, Mp, MaxMp); }
    }

    public int Atk {
        get { return m_atk; }
        set { m_atk = value; Send.SendMsg(SendType.AtkChange, Atk); }
    }

    public int Gold {
        get { return m_gold; }
        set { m_gold = value; Send.SendMsg(SendType.GoldChange, Atk); }
    }

    public void SetData(RoleType _roleType) {
        roleType = _roleType;
        Lv = 1;
        RefRole refRole = RefRole.GetRef(roleType);
        MaxHp = refRole.Hp;
        Hp = MaxHp;
        MaxMp = refRole.MaxMp;
        Mp = refRole.DefaultMp;
        Atk = refRole.Atk;
        Gold = 0;
        skillList.Clear();
        if (refRole.Skill != 0)
            AddSkill(refRole.Skill);

        if (refRole.Item != 0)
            AddItem(refRole.Item, refRole.ItemNum);
    }

    public void AddSkill(int _SkillId) {
        if (!RefSkill.HasKey(_SkillId)) {
            Debug.LogError("skillId is not find:" + _SkillId);
            return;
        }
        skillList.Add(_SkillId);

        Send.SendMsg(SendType.SkillChange, skillList);
    }

    public void AddItem(int _id, int _num ) {
        if (!RefItem.HasKey(_id)) {
            Debug.LogError("RefItem is not find:" + _id);
            return;
        }
        for (int index = 0; index < itemList.Count; index++ ) {
            ItemData data = itemList[index];
            if (data.data.Id == _id) {
                int remain = data.remainNum();
                if (_num <= remain) {
                    data.Num += _num;
                    _num = 0;
                    break;
                }
                else {
                    data.Num += remain;
                    _num -= remain;
                }
            }
        }

        if (_num != 0) {
            ItemData data = new ItemData();
            data.Init(_id, _num);
            itemList.Add(data);
        }

        Send.SendMsg(SendType.ItemChange, itemList);
    }

    public void UseItem(ItemData item) {
        item.Num--;
        if (item.Num == 0) {
            itemList.Remove(item);
        }

        Send.SendMsg(SendType.ItemChange, itemList);
    }

    public void Equip(EquipData equipData) {
        int index = (int)equipData.data.position;
        arrEquip[index] = equipData;

        Send.SendMsg(SendType.EquipChange, arrEquip);
    }
}
