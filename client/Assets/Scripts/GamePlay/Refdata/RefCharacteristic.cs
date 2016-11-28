using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefCharacteristic : RefBase {

    public static Dictionary<int, RefCharacteristic> cacheMap = new Dictionary<int, RefCharacteristic>();
    public int ID;
    public string Desc;
    public List<int> ResultIDList;

    public override string GetFirstKeyName() {
        return "ID";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        ID = GetInt("ID");
        Desc = GetString("Desc");
        ResultIDList = GetList<int>("ResultIDList", typeof(int));
    }

    public static RefCharacteristic GetRef(int _id) {
        RefCharacteristic data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefAction key:" + _id);
        }
        return data;
    }
}
