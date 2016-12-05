using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefGeneral : RefBase {
    public static Dictionary<string, RefGeneral> cacheMap = new Dictionary<string, RefGeneral>();

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

    static RefGeneral GetRef(string key) {
        RefGeneral _data = null;
        if (!cacheMap.TryGetValue(key, out _data)) {
            Debug.LogError("not find key :" + key);
        }
        return _data;
    }

    public static int GetIntValue(string key, int defaultValue = 0) {
        RefGeneral _data = GetRef(key);
        if (_data != null) {
            string value = _data.Value;
            return System.Int32.Parse(value);
        }
        return defaultValue;
    }

    public static float GetFloatValue(string key, float defaultValue = 0f) {
        RefGeneral _data = GetRef(key);
        if (_data != null) {
            string value = _data.Value;
            return float.Parse(value);
        }
        return defaultValue;
    }

    public static bool GetBoolValue(string key, bool defaultValue = false) {
        RefGeneral _data = GetRef(key);
        if (_data != null) {
            string value = _data.Value;
            return bool.Parse(value);
        }
        return defaultValue;
    }

    public static string GetStringValue(string key, string defaultValue = "") {
        RefGeneral _data = GetRef(key);
        if (_data != null) {
            return _data.Value;
        }
        return defaultValue;
    }
}


