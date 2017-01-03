using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefItem : RefBase {
    public static Dictionary<int, RefItem> cacheMap = new Dictionary<int, RefItem>();

    public int Id;
    public string Icon;
    public int MaxNum;

    public override string GetFirstKeyName() {
        return "Id";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Id = GetInt("Id");
        Icon = GetString("Icon");
        MaxNum = GetInt("MaxNum");
    }

    public static RefItem GetRef(int _id) {
        RefItem data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefItem key:" + _id);
        }
        return data;
    }

    public static bool HasKey(int _id) {
        return cacheMap.ContainsKey(_id);
    }
}
