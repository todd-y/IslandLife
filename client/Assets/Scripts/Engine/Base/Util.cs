using UnityEngine;
using System.Collections;

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
}
