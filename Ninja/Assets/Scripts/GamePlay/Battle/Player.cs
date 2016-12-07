using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float fixY = 1;

    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private AnimCtrl animCtrl;
    private GameObject prefab;
    private PlayerBack playBack;

    private int hp = 3;
    private float speed = 5f;
    private float fireCD = 0.5f;
    private float lastFireTime = 0;
    private Vector2 tempVector2 = Vector2.zero;
	
    // system function
    void Start () {
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator != null) {
            animCtrl = new AnimCtrl(animator);
        }
        else {
            Debug.LogError("animator is null :" + gameObject.name);
        }
	}
	
	void Update () {
        InputHandle();
	}

    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    //self function
    public void SetDefault() {
        hp = 3;
    }

    private void InputHandle() {
        tempVector2.x = Input.GetAxisRaw(AXIS_HORIZONTAL);
        tempVector2.y = Input.GetAxisRaw(AXIS_VERTICAL);
        
        if (tempVector2 == Vector2.zero) {
            animCtrl.StopWalk();
        }
        else {
            Move(tempVector2.normalized);
        }
        if (Input.GetMouseButtonDown(0)) {
            Fire();
        }

        FaceHandle();
    }

    private void FaceHandle() {
        Vector2 playerPos = gameObject.transform.position;
        playerPos.y = playerPos.y + fixY;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.SetLocalScaleX(mousePos.x - playerPos.x > 0 ? -1 : 1);
    }

    private void Move(Vector2 direction) {
        Vector2 pos = transform.localPosition;

        pos += direction * speed * Time.deltaTime;
        transform.localPosition = pos;
        animCtrl.PlayWalk();
    }

    private void Fire() {
        if (prefab == null) {
            prefab = LocalAssetMgr.Instance.Load_Prefab("PlayerBack");
            playBack = prefab.GetComponent<PlayerBack>();
        }
        if (Time.time > (lastFireTime + fireCD) ) {
            lastFireTime = Time.time;
            Vector2 playerPos = gameObject.transform.position;
            playerPos.y = playerPos.y + fixY;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Vector2.Angle(Vector2.right, mousePos - playerPos);
            angle = mousePos.y > playerPos.y ? angle : -angle;
            UbhObjectPool.Instance.GetGameObject(prefab, new Vector3(playerPos.x, playerPos.y, 0),
                                                                            Quaternion.Euler(0, 0, angle));

            animCtrl.PlayAttack();
        }
    }

    private void HitCheck(Transform colTrans) {
        int colLayer = colTrans.gameObject.layer;
        if (colLayer == GeneralDefine.EnemyBulletLayer || colLayer == GeneralDefine.EnemyLayer) {
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
