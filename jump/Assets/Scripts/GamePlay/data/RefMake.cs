using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefMake : RefBase {
    public static Dictionary<int, RefMake> cacheMap = new Dictionary<int, RefMake>();

    public int Id;
    public MakeType type;
    public int NeedNum;

    public override string GetFirstKeyName() {
        return "Id";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Id = GetInt("Id");
    }

    public static RefMake GetRef(int _id) {
        RefMake data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefMake key:" + _id);
        }
        return data;
    }
}
