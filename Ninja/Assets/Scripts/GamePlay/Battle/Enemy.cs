using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public Transform target;
    private int hp = 10;
    private AnimCtrl animCtrl;

    // system function
    void Start() {
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator != null) {
            animCtrl = new AnimCtrl(animator);
        }
        else {
            Debug.LogError("animator is null :" + gameObject.name);
        }

        target = UbhUtil.GetTransformFromTagName("Player");
    }
    
    void Update() {
        FaceTarget();
    }

    void FaceTarget() {
        if (target == null)
            return;

        transform.SetLocalScaleX(target.position.x - transform.position.x > 0 ? -1 : 1);
    }
    
    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    private void HitCheck(Transform colTrans) {
        int colLayer = colTrans.gameObject.layer;
        if (colLayer == GeneralDefine.PlayerBulletLayer) {
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);
            Injury();
        }
    }

    private void Injury() {
        hp--;
        if (hp > 0) {
            if (animCtrl != null) animCtrl.PlayHit();
        }
        else {
            if (animCtrl != null) animCtrl.PlayDeath();
        }
    }
}
