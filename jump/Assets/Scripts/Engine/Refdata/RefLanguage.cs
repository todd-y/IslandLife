using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefLanguage : RefBase {

    public static Dictionary<string, RefLanguage> cacheMap = new Dictionary<string, RefLanguage>();

    public string Key;
    public string Value;

    public override string GetFirstKeyName() {
        return "Key";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Key = GetString("Key");
        Value = GetString("Value");
    }

    public static string GetValue(string _key){
        if (string.IsNullOrEmpty(_key))
            return "";

        RefLanguage data = null;
        if (cacheMap.TryGetValue(_key, out data)) {
            return data.Value;
        }

        //if (data == null) {
        //    Debug.Log("error RefLanguage key:" + _key);
        //}
        return _key;
    }

    public static string GetValueParam(string _key, params object[] _obj) {
        string ret = GetValue(_key);
        if (null == ret)
            return ret;
        // 字符串替换
        return string.Format(ret, _obj);
    }
}
