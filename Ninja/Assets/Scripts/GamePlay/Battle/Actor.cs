using UnityEngine;
using System.Collections;

public class Actor : UbhMonoBehaviour {
    //protected AnimCtrl animCtrl;
    private Collider2D collider;
    protected SpriteRenderer bodyRenderer;
    private SpriteRenderer flashRenderer;

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
            maxMp = value;
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
        bodyRenderer = gameObject.GetChildControl<SpriteRenderer>("Body");
        flashRenderer = gameObject.GetChildControl<SpriteRenderer>("HighLight");

        collider = gameObject.GetComponent<CircleCollider2D>();
        SetColliderState(true);
    }

    protected virtual void HitCheck(Transform colTrans) {
    }

    protected virtual void Injury(int damageValue = 1) {
        CurHp -= damageValue;
        FlashAnim();
    }

    protected virtual void BirthHandle() {
        SetColliderState(true);
        alive = true;
    }

    protected virtual void DeadHandle() {
        SetColliderState(false);
        alive = false;
    }

    protected void SetColliderState(bool _state) {
        if (collider == null)
            return;

        collider.enabled = _state;
    }

    protected void Move(Vector2 direction) {
        Vector2 start = transform.position;
        Vector2 end = start + direction * speed * Time.deltaTime;
        rigidbody2D.MovePosition(end);
    }

    public void SetBasicInfo(float hpValue, float mpValue = 100) {
        MaxHp = hpValue;
        CurHp = hpValue;

        MaxMp = mpValue;
        CurMp = 0;
    }

    protected virtual void FlashAnim() {
        StartCoroutine(CO_FlashAnim());
    }

    private IEnumerator CO_FlashAnim() {
        if (flashRenderer == null)
            yield break;
        Color flashColor = Color.white;
        float startAlpha = 0;
        float endAlpha = 0.8f;
        flashColor.a = startAlpha;
        flashRenderer.color = flashColor;

        float totalTime = 0.2f;
        float halfTime = totalTime / 2;
        float curTime = 0f;

        while(curTime <= totalTime){
            float newA = Mathf.Lerp(startAlpha, endAlpha, 
                curTime <= halfTime ? curTime / halfTime : (totalTime - curTime) / halfTime);
            if(roleType == RoleType.Monster)
            flashColor.a = newA;
            flashRenderer.color = flashColor;
            curTime += Time.deltaTime;
            yield return null;
        }

        flashColor.a = startAlpha;
        flashRenderer.color = flashColor;
    }
}
