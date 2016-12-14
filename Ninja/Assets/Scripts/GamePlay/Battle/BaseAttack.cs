using UnityEngine;
using System.Collections;

public class BaseAttack : MonoBehaviour {
    public float time = 0.5f;
    private float curTime = 0;

    void FixedUpdate() {
        if (time <= 0)
            return;

        curTime += Time.fixedDeltaTime;
        if (curTime >= time) {
            curTime = 0f;
            UbhObjectPool.Instance.ReleaseGameObject(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    protected virtual void HitCheck(Transform colTrans) {
        
    }
}
