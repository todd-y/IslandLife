using UnityEngine;
using System.Collections;

public class BulletBack : MonoBehaviour {
    public const string NAME_ENEMY_BULLET = "EnemyBullet";
    public const string NAME_ENEMY = "Enemy";

    public AudioSource _AudioShot;

    void OnAnimationFinish() {
        UbhObjectPool.Instance.ReleaseGameObject(gameObject);
    }

    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    void HitCheck(Transform colTrans) {
        ObjInfo colInfo = colTrans.GetComponent<ObjInfo>();
        if (colInfo == null) {
            Debug.LogError("col info is null:" + colTrans.name);
            return;
        }

        switch (colInfo.objType) {
            case ObjType.EnemyBullet:
                UbhBullet ubhBullet = colTrans.GetComponent<UbhBullet>();
                if (ubhBullet == null) {
                    ubhBullet = colTrans.parent.GetComponent<UbhBullet>();
                }
                if (ubhBullet == null) {
                    Debug.LogError("ubhbullet is null:" + colTrans.name);
                    return;
                }
                colInfo.objType = ObjType.BackBullet;
                ubhBullet.ReturnBullet();

                if (_AudioShot != null) {
                    _AudioShot.Play();
                }
                break;
            default:
                break;
        }
    }
}
