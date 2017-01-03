using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefRole : RefBase {
    public static Dictionary<RoleType, RefRole> cacheMap = new Dictionary<RoleType, RefRole>();

    public RoleType RoleType;
    public int Hp;
    public int DefaultMp;
    public int MaxMp;
    public int Atk;
    public int Skill;
    public int Item;
    public int ItemNum;

    public override string GetFirstKeyName() {
        return "RoleType";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        RoleType = (RoleType)GetEnum("RoleType", typeof(RoleType));
        Hp = GetInt("Hp");
        DefaultMp = GetInt("DefaultMp");
        MaxMp = GetInt("MaxMp");
        Atk = GetInt("Atk");
        Skill = GetInt("Skill");
        Item = GetInt("Item");
        ItemNum = GetInt("ItemNum");
    }

    public static RefRole GetRef(RoleType _id) {
        RefRole data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefRole key:" + _id);
        }
        return data;
    }
}
