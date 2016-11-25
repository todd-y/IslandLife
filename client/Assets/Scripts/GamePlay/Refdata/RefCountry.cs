using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefCountry : RefBase {

    public static Dictionary<int, RefCountry> cacheMap = new Dictionary<int, RefCountry>();

    public int ID;
    public int RemainFood;
    public int TaxRate;
    public int ArmyRate;
    public List<int> King;
    public int CountyNum;
    public int WifeNum;
    public int MinisterNum;
    public int SecretAgentNum;

    public override string GetFirstKeyName() {
        return "ID";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        ID = GetInt("ID");
        RemainFood = GetInt("RemainFood");
        TaxRate = GetInt("TaxRate");
        ArmyRate = GetInt("ArmyRate");
        King = GetList<int>("King", typeof(int));
        CountyNum = GetInt("CountyNum");
        WifeNum = GetInt("WifeNum");
        MinisterNum = GetInt("MinisterNum");
        SecretAgentNum = GetInt("SecretAgentNum");
    }

    public static RefCountry GetRef(int _id) {

        RefCountry data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefIcon key:" + _id);
        }
        return data;
    }
}
