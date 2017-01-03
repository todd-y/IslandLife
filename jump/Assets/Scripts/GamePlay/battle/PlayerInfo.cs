using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfo {
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
    public List<int> itemList = new List<int>();

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
        skillList.Add(refRole.Skill);
    }
}
