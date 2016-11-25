using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefCounty : RefBase {

    public static Dictionary<int, RefCounty> cacheMap = new Dictionary<int, RefCounty>();

    public int ID;
    public List<int> PeopleNum;
    public List<int> Loyalty;
    public List<int> RemainFood;
    public List<int> ArmyNum;
    public List<int> CorruptionRate;
    public List<int> AreaFactor;

    public override string GetFirstKeyName() {
        return "ID";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        ID = GetInt("ID");
        PeopleNum = GetList<int>("PeopleNum", typeof(int));
        Loyalty = GetList<int>("Loyalty", typeof(int));
        RemainFood = GetList<int>("RemainFood", typeof(int));
        ArmyNum = GetList<int>("ArmyNum", typeof(int));
        CorruptionRate = GetList<int>("CorruptionRate", typeof(int));
        AreaFactor = GetList<int>("AreaFactor", typeof(int));
    }

    public static RefCounty GetRef(int _id) {

        RefCounty data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefIcon key:" + _id);
        }
        return data;
    }
}
