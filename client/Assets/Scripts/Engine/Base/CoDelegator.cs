using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CoDelegator : MonoBehaviour {

	private static CoDelegator _instance = null;
    public static CoDelegator Instance {
        get {
            return _instance;
        }
        set {
            _instance = value;
        }
    }

    // Unity协程
    public static Coroutine Coroutine (IEnumerator routine) {
        return Instance.StartCoroutine(routine);
    }

    // Unity协程
    public static void StopCoroutineEx (string _routine) {
        Instance.StopCoroutine(_routine);
    }
}
