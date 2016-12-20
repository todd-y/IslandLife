using UnityEngine;
using System.Collections;

public class Actor : UbhMonoBehaviour {
    //protected AnimCtrl animCtrl;
    private CircleCollider2D collider;
    private SpriteRenderer bodyRenderer;
    private SpriteRenderer colorRenderer;
    protected _2dxFX_DesintegrationFX colorFX;
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
        //Animator animator = gameObject.GetComponent<Animator>();
        //if (animator != null) {
        //    animCtrl = new AnimCtrl(animator);
        //}
        //else {
        //    Debug.LogError("animator is null :" + gameObject.name);
        //}

        bodyRenderer = gameObject.GetChildControl<SpriteRenderer>("Body");
        colorRenderer = gameObject.GetChildControl<SpriteRenderer>("ColorChange");
        colorFX = colorRenderer.GetComponent<_2dxFX_DesintegrationFX>();
        colorFX.Desintegration = 1;
        colorFX.Seed = Random.Range(0, 1f);
        flashRenderer = gameObject.GetChildControl<SpriteRenderer>("HighLight");

        collider = gameObject.GetComponent<CircleCollider2D>();
        collider.enabled = true;
    }

    protected void SetBodyColor(Color color) {
        bodyRenderer.color = color;
    }

    protected void SetTargetColor(Color color) {
        colorFX._Color = color;
        colorRenderer.color = color;
    }

    protected virtual void HitCheck(Transform colTrans) {
    }

    protected virtual void Injury(int damageValue = 1) {
        CurHp -= damageValue;
        //if (CurHp > 0) {
        //    if (animCtrl != null) animCtrl.PlayHit();
        //}
        FlashAnim();
        ChangeColor();
    }

    protected virtual void BirthHandle() {
        collider.enabled = true;
        alive = true;
    }

    protected virtual void DeadHandle() {
        //if (animCtrl != null) animCtrl.PlayDeath();
        collider.enabled = false;
        alive = false;
    }

    protected void Move(Vector2 direction) {
        Vector2 start = transform.position;
        Vector2 end = start + direction * speed * Time.deltaTime;
        //RaycastHit2D hit = Physics2D.BoxCast(start + collider.offset, collider.bounds.size, 0, direction,
        //                                Vector2.Distance(end, start), GeneralDefine.CannotMoveMask);
        //if (hit.transform == null) {
        rigidbody2D.MovePosition(end);
        //animCtrl.PlayWalk();
        //}
    }

    public void SetBasicInfo(float hpValue, float mpValue = 100) {
        MaxHp = hpValue;
        CurHp = hpValue;

        MaxMp = mpValue;
        CurMp = mpValue;
    }

    public void FlashAnim() {
        StopCoroutine(CO_FlashAnim());
        StartCoroutine(CO_FlashAnim());
    }

    private IEnumerator CO_FlashAnim() {
        Color flashColor = Color.white;
        float startAlpha = 0;
        float endAlpha = 200;
        flashColor.a = startAlpha;
        flashRenderer.color = flashColor;

        float totalTime = 0.1f;
        float halfTime = 0.05f;
        float curTime = 0f;

        while(curTime <= totalTime){
            float newA = Mathf.Lerp(startAlpha, endAlpha, 
                curTime <= halfTime ? curTime / halfTime : (totalTime - curTime) / halfTime);

            flashColor.a = newA;
            flashRenderer.color = flashColor;
            curTime += Time.deltaTime;
            yield return null;
        }

        flashColor.a = startAlpha;
        flashRenderer.color = flashColor;
    }

    protected virtual void ChangeColor() {
        float per = 0.2f + CurHp / MaxHp * 0.5f;
        colorFX.Desintegration = per;
    }
}
