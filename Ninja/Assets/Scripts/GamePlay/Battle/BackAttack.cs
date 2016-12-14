using UnityEngine;
using System.Collections;

public class BackAttack : BaseAttack {
    public GameObject prefab;

    protected override void HitCheck(Transform colTrans) {
        if (colTrans.gameObject.layer == GeneralDefine.EnemyBulletLayer) {
            UbhBullet ubhBullet = colTrans.GetComponent<UbhBullet>();
            if (ubhBullet == null) {
                ubhBullet = colTrans.parent.GetComponent<UbhBullet>();
            }
            if (ubhBullet == null) {
                Debug.LogError("ubhbullet is null:" + colTrans.name);
                return;
            }
            Quaternion newBulletRotation = Quaternion.Euler( transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90);
            UbhObjectPool.Instance.GetGameObject(prefab, colTrans.position, newBulletRotation );
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);
        }
    }
}
