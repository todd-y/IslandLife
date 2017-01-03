using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefSkill : RefBase {
    public static Dictionary<int, RefSkill> cacheMap = new Dictionary<int, RefSkill>();

    public int Id;
    public string Icon;
    public int Cost;

    public override string GetFirstKeyName() {
        return "Id";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Id = GetInt("Id");
        Icon = GetString("Icon");
        Cost = GetInt("Cost");
    }

    public static RefSkill GetRef(int _id) {
        RefSkill data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefSkill key:" + _id);
        }
        return data;
    }
}
