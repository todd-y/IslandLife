using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    private Collider2D collider;
    protected AnimCtrl animCtrl;
    protected int hp;

    protected bool alive = true;

    void Start() {
        Init();
        BirthHandle();
    }

    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    private void Init(){
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator != null) {
            animCtrl = new AnimCtrl(animator);
        }
        else {
            Debug.LogError("animator is null :" + gameObject.name);
        }

        collider = gameObject.GetComponent<Collider2D>();
        collider.enabled = true;
    }

    protected virtual void HitCheck(Transform colTrans) {
    }

    protected void Injury(int damageValue = 1) {
        hp -= damageValue;
        if (hp > 0) {
            if (animCtrl != null) animCtrl.PlayHit();
        }
        else {
            DeadHandle();
        }
    }

    protected virtual void BirthHandle() {
        collider.enabled = true;
        alive = true;
    }

    protected virtual void DeadHandle() {
        if (animCtrl != null) animCtrl.PlayDeath();
        collider.enabled = false;
        alive = false;
    }
}
