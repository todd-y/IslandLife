using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CoDelegator : SingletonMonoBehavior<CoDelegator> {
    // Unity协程
    public static Coroutine Coroutine (IEnumerator routine) {
        return Instance.StartCoroutine(routine);
    }

    public static void StopCoroutineEx(IEnumerator routine) {
        Instance.StopCoroutine(routine);
    }

    // Unity协程
    public static void StopCoroutineEx (string _routine) {
        Instance.StopCoroutine(_routine);
    }
}
