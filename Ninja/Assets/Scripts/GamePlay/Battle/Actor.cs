using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    protected AnimCtrl animCtrl;
    private Collider2D collider;

    private float speed = 10f;
    public RoleType roleType;
    private float curHp = 100;
    public float CurHp {
        get { return curHp; }
        set {
            curHp = Mathf.Clamp(value, 0, MaxHp);
            if (roleType == RoleType.Player) {
                Send.SendMsg(SendType.PlayerHpChange, curHp, MaxHp);
            }
            if (curHp <= 0) {
                DeadHandle();
            }
        }
    }

    private float maxHp = 100;
    public float MaxHp {
        get { return maxHp; }
        set { maxHp = value; }
    }

    protected bool alive = true;

    private float curMp = 100;
    public float CurMp {
        get { return curMp; }
        set {
            curMp = Mathf.Clamp(value, 0, MaxMp);
            Send.SendMsg(SendType.PlayerMpChange, curMp, MaxMp);
        }
    }

    private float maxMp = 100;
    public float MaxMp {
        get { return maxMp; }
        set {
            maxHp = value;
        }
    }

    void Start() {
        InitComponent();
        BirthHandle();
    }

    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    private void InitComponent(){
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
        CurHp -= damageValue;
        if (CurHp > 0) {
            if (animCtrl != null) animCtrl.PlayHit();
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

    public void SetBasicInfo(float hpValue, float mpValue = 100) {
        MaxHp = hpValue;
        CurHp = hpValue;

        MaxMp = mpValue;
        CurMp = mpValue;
    }
}
