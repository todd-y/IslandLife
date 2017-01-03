using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefLv : RefBase {
    public static Dictionary<int, RefLv> cacheMap = new Dictionary<int, RefLv>();

    public int Lv;
    public int NextLvExp;

    public override string GetFirstKeyName() {
        return "Lv";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Lv = GetInt("Lv");
        NextLvExp = GetInt("NextLvExp");
    }

    public static RefLv GetRef(int _id) {
        RefLv data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefIcon key:" + _id);
        }
        return data;
    }

    public static int GetNextLvExp(int _lv) {
        RefLv refLv = RefLv.GetRef(_lv);
        if (refLv == null) {
            Debug.LogError("target Lv is none");
            return _lv * 10;
        }
        else {
            return refLv.NextLvExp;
        }
    }
}
