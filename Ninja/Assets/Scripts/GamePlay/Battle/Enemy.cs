using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public Transform target;
    public List<UbhBaseShot> shotList = new List<UbhBaseShot>();
    private int hp = 10;
    private AnimCtrl animCtrl;
    private EnemyAI ai;
    private UbhBaseShot curShot;
    private bool alive = true;

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

        ai = new EnemyAI(this);
        ai.IsAwake(true);
    }
    
    void Update() {
        FaceTarget();
        ai.Update();
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
        if (alive == false)
            return;
        hp--;
        if (hp > 0) {
            if (animCtrl != null) animCtrl.PlayHit();
        }
        else {
            if (animCtrl != null) animCtrl.PlayDeath();
            ai.IsAwake(false);
            if (curShot != null) curShot.FinishedShot();
            alive = false;
        }
    }

    public void Shot(int index) {
        if (curShot!= null && curShot.Shooting) {
            return;
        }
        curShot = shotList[index];
        curShot.Shot();
    }

    public bool CanShot() {
        if(curShot == null)
            return true;
        return !curShot.Shooting;
    }
}
