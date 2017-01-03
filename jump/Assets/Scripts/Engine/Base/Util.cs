using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

static public class Util {
    static public T AddMissingComponent<T>(this GameObject go) where T : Component {
        T comp = go.GetComponent<T>();
        if (comp == null) {
            comp = go.AddComponent<T>();
        }
        return comp;
    }

    static public T AddMissingComponent<T>(this Component _comp) where T : Component {
        T result = null;
        if (_comp != null) {
            result = _comp.gameObject.AddMissingComponent<T>();
        }
        return result;
    }

    static public void SetText (this Text text, string str)  {
        if (text != null) {
            text.text = RefLanguage.GetValue(str);
        }
    }

    static public void SetText(this Text text, string str1, int str2) {
        if (text != null) {
            text.text = RefLanguage.GetValue(str1) + str2;
        }
    }

    static public void SetText(this Text text, float str) {
        if (text != null) {
            text.text = str.ToString();
        }
    }

    static public void SetText(this Text text, DateTime str) {
        if (text != null) {
            text.text = string.Format("{0}-{1}-{2}", str.Year, str.Month, str.Day);
        }
    }

    static public T GetChildControl<T>(this GameObject _obj, string _target) where T : Component {
        Transform child = _obj.transform.Find(_target);
        if (child != null) {
            return child.GetComponent<T>();
        }
        //Debug.LogWarning("查找不到你所要找到" + _target + " 控件");
        return null;
    }
}
