using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefActor : RefBase {
    public static Dictionary<int, RefActor> cacheMap = new Dictionary<int, RefActor>();

    public int ID;
    public List<int> RemainFood;
    public List<int> Age;
    public List<int> Loyalty;
    public List<int> Cachet;
    public List<int> Ability;

    public override string GetFirstKeyName() {
        return "ID";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        ID = GetInt("ID");
        Loyalty = GetList<int>("Loyalty", typeof(int));
        RemainFood = GetList<int>("RemainFood", typeof(int));
        Age = GetList<int>("Age", typeof(int));
        Cachet = GetList<int>("Cachet", typeof(int));
        Ability = GetList<int>("Ability", typeof(int));
    }

    public static RefActor GetRef(int _id) {

        RefActor data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefIcon key:" + _id);
        }
        return data;
    }
}
