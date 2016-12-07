using UnityEngine;
using System.Collections;

public class PlayerBack : MonoBehaviour {
    public float time = 0.5f;
    private float curTime = 0;

    void FixedUpdate() {
        curTime += Time.fixedDeltaTime;
        if (curTime >= time) {
            curTime = 0f;
            UbhObjectPool.Instance.ReleaseGameObject(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    private void HitCheck(Transform colTrans) {
        if (colTrans.gameObject.layer == GeneralDefine.EnemyBulletLayer) {
            UbhBullet ubhBullet = colTrans.GetComponent<UbhBullet>();
            if (ubhBullet == null) {
                ubhBullet = colTrans.parent.GetComponent<UbhBullet>();
            }
            if (ubhBullet == null) {
                Debug.LogError("ubhbullet is null:" + colTrans.name);
                return;
            }
            ubhBullet.ReturnBullet();
        }
    }
}
