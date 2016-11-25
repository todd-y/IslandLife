using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefName : RefBase {

    public static Dictionary<int, RefName> cacheMap = new Dictionary<int, RefName>();

    public int Key;
    public string Value;
    public NameType NameType;

    public override string GetFirstKeyName() {
        return "Key";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Key = GetInt("Key");
        Value = GetString("Value");
        NameType = (NameType)GetEnum("NameType", typeof(NameType));
    }

    public static List<string> GetTypeList(NameType type) {
        List<string> list = new List<string>();
        foreach (RefName name in cacheMap.Values) {
            if (name.NameType == type) {
                list.Add(name.Value);
            }
        }

        return list;
    }

    public static string GetRandomName(NameType type) {
        List<string> list = GetTypeList(type);
        return GetRandomName(list);
    }

    public static string GetRandomName(List<string> list) {
        if (list.Count == 0) {
            Debug.LogError("list is null");
            return "";
        }

        return list[Random.Range(0, list.Count)];
    }

    public static string GetActorRandomName(){
        return GetRandomName(NameType.FamilyName) + GetRandomName(NameType.FirstName);
    }
}
