using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    private Collider2D collider;
    private float speed = 5f;
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

    protected void Move(Vector2 direction) {
        Vector2 start = transform.position;
        Vector2 end = start + direction * speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.BoxCast(start + collider.offset, collider.bounds.size, 0, direction, 
                                        Vector2.Distance(end, start), GeneralDefine.CannotMoveMask);
        if (hit.transform == null) {
            transform.localPosition = end;
            animCtrl.PlayWalk();
        }
    }
}
