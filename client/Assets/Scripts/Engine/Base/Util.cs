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

    static public void setText (this Text text, string str)  {
        if (text != null) {
            text.text = RefLanguage.GetValue(str);
        }
    }

    static public void setText(this Text text, string str1, int str2) {
        if (text != null) {
            text.text = RefLanguage.GetValue(str1) + str2;
        }
    }

    static public void setText(this Text text, float str) {
        if (text != null) {
            text.text = str.ToString();
        }
    }

    static public void setText(this Text text, DateTime str) {
        if (text != null) {
            text.text = string.Format("{0}-{1}-{2}", str.Year, str.Month, str.Day);
        }
    }
}
