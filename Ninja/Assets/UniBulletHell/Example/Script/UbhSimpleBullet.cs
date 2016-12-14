using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class UbhSimpleBullet : UbhMonoBehaviour
{
    public float _Speed = 10;

    void OnEnable ()
    {
        rigidbody2D.velocity = transform.up.normalized * _Speed;
    }

    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    private void HitCheck(Transform colTrans) {
        int colLayer = colTrans.gameObject.layer;
        if (colLayer == GeneralDefine.WallLayer) {
            UbhObjectPool.Instance.ReleaseGameObject(gameObject);
        }
    }
}