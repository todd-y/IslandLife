﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefDataText {
    public string fulltext = "";
}

public class BaseRefDataMgr<T> : Singleton<T> where T : ISingleton, new() {

    protected static int pendings = 0;
    // 1 声明
    //private Dictionary<int, refChooseAvatar> chooseAvatar = new Dictionary<int, refChooseAvatar>();
    //private Dictionary<string, refWindow> windows = new Dictionary<string, refWindow>();

    // 2 接口
    //public Dictionary<int, refChooseAvatar> getChooseAvatar() { return chooseAvatar; }
    //public Dictionary<string, refWindow> getWindow() { return windows; }

    // 3 加载
    //public void LoadAll() {
    //LoadGeneric(chooseAvatar);
    //LoadGeneric(windows);
    //}

    /************************************************************************/
    /* function                                                                     */
    /************************************************************************/
    //  [4/10/2014 cheney]

    protected static IEnumerator Co_LoadText (string tableName, RefDataText tableText, Dictionary<int, long> _table, bool fromLocal = false) {
        bool complete = false;
        tableName = tableName.ToLower();
        //LocalAssetMgr.Instance.Load_RefData(tableName,
        //        delegate(TextAsset asset) {
        //            tableText.fulltext = asset.text;
        //            complete = true;
        //        },
        //        fromLocal);

        while ( !complete )
            yield return null;

        string[] stringReader = tableText.fulltext.Split('\n');
        long index = 0;
		
		for (int i = 0; i < stringReader.Length; i++) {
			string line = stringReader[i];
			if (i >= 2 && line.Trim() != string.Empty)
			{
				string[] words = line.Split('\t');
				int id = int.Parse(words[0]);
				_table[id] = (index << 32) + line.Length;
			}
            index += line.Length + 1;
        }
    }

    protected static IEnumerator Co_LoadGeneric<TKey, TValue> (Dictionary<TKey, TValue> _table, bool fromLocal = false) where TValue : RefBase {
        pendings += 1;
        Type _valueType = typeof(TValue);
        // 读二维表
        // 从D2TableManager 获取
        string tableName = _valueType.Name.Substring(3).ToLower();

        //LocalAssetMgr.Instance.Load_RefData(tableName,
        //        delegate(TextAsset asset) {
        //            Dictionary<string, Dictionary<string, string>> tableInfo = D2TableMgr.LoadTable(asset);
        //            if (D2TableMgr.LoadByTable(_table, tableInfo)) {
        //                pendings -= 1;
        //            }
        //        },
        //        fromLocal);

        while (pendings > 0) {
            yield return null;
        }
    }

    public static void LoadGeneric<TKey, TValue> (Dictionary<TKey, TValue> _table, bool fromLocal = false) where TValue : RefBase {
        pendings += 1;
        Type _valueType = typeof(TValue);
        // 读二维表
        // 从D2TableManager 获取
        string tableName = _valueType.Name.Substring(3).ToLower();
        Debug.Log(string.Format("LoadTable:<{0}> start ...", tableName));

        //LocalAssetMgr.Instance.Load_RefData(tableName,
        //        delegate(TextAsset asset) {
        //            Dictionary<string, Dictionary<string, string>> tableInfo = D2TableMgr.LoadTable(asset);
        //            if (D2TableMgr.LoadByTable(_table, tableInfo)) {
        //                pendings -= 1;
        //            }
        //        },
        //        fromLocal);
    }

    // 解析字段值
    private static object ParseValue (string _value, Type _type) {
        try {
            if (_value.Equals(string.Empty)) {
                if (_type == typeof(string)) {
                    return "";
                }
                return Activator.CreateInstance(_type);
            }
            else {
                _value = _value.Trim();

                // 枚举 暂不支持
                if (_type.IsEnum) {
                    return Enum.Parse(_type, _value, true);
                }

                // 字符串
                else if (_type == typeof(string)) {
                    return _value;
                }

                // 浮点型
                else if (_type == typeof(float)) {
                    if (_value == "0" || _value == "" || _value == string.Empty)
                        return 0;

                    return float.Parse(_value);
                }

                // 整形
                else if (_type == typeof(int)) {
                    if (_value == "")
                        return 0;

                    return int.Parse(_value);
                }

                else if (_type == typeof(bool)) {
                    return bool.Parse(_value);
                }

                else if (_type == typeof(long)) {
                    return long.Parse(_value);
                }
            }
        }
        catch (System.Exception ex) {
            Debug.LogError(string.Format("ParseValue type:{0}, value:{1}, failed: {2}", _type.ToString(), _value, ex.Message));
        }
        return null;
    }
}
