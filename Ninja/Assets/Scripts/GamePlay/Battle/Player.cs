using UnityEngine;
using System.Collections;
using DunGen;

public class Player : Actor {
    public float fixY = 1;
    public UbhBaseShot curShot;

    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private GameObject rightClickPrefab;
    private GameObject rightClickGo;

    private Vector2 tempVector2 = Vector2.zero;
    private bool isRightClick = false;

    private float rightCost = 20;
    private float mpRecovery = 20;
    private float minRightTime = 1;
    private float rightClickTime = 0;
    private Weapeon leftWeapeon;
    private Weapeon injuryWeapon; 

    public Player() {
        roleType = RoleType.Player;
    }

    // system function
    void FixedUpdate() {
        InputHandle();
        MpRecoveryHandle();
	}

    //self function
    protected override void BirthHandle() {
        base.BirthHandle();
        SetBasicInfo(100);
        LoadWeapeon();
    }

    private void LoadWeapeon() {
        GameObject leftGo = GameObject.Instantiate(LocalAssetMgr.Instance.Load_Prefab("GunLiner"));
        leftGo.transform.SetParent(transform, false);        
        leftWeapeon = leftGo.GetComponent<Weapeon>();


        GameObject injuryGo = GameObject.Instantiate(LocalAssetMgr.Instance.Load_Prefab("GunCircle"));
        injuryGo.transform.SetParent(transform, false);
        injuryWeapon = injuryGo.GetComponent<Weapeon>();
    }

    private void InputHandle() {
        if (alive == false)
            return;
        tempVector2.x = Input.GetAxisRaw(AXIS_HORIZONTAL);
        tempVector2.y = Input.GetAxisRaw(AXIS_VERTICAL);
        
        if (tempVector2 == Vector2.zero) {
            animCtrl.StopWalk();
        }
        else {
            Move(tempVector2.normalized);
        }
        if (Input.GetMouseButton(0)) {
            LeftFire();
        }

        if (Input.GetMouseButton(1)) {
            bool needRecordTime = !isRightClick;
            if (isRightClick && CurMp > 0) {
               isRightClick = true;
            }
            else if(CurMp >= rightCost) {
                isRightClick = true;
            }
            else {
                isRightClick = false;
            }

            if (isRightClick && needRecordTime) {
                rightClickTime = Time.time;
            }
        }
        else {
            if (isRightClick && (Time.time - rightClickTime) > minRightTime || CurMp <= 0) {
                isRightClick = false;
                RightRelease();
            }
        }

        if (isRightClick) {
            RightHold();
        }
        else {
            RightRelease();
        }

        FaceHandle();
    }

    private void FaceHandle() {
        Vector2 playerPos = gameObject.transform.position;
        playerPos.y = playerPos.y + fixY;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.SetLocalScaleX(mousePos.x - playerPos.x > 0 ? -1 : 1);
    }

    private void LeftFire() {
        leftWeapeon.Shot();
        //if (leftClickPrefab == null) {
        //    leftClickPrefab = LocalAssetMgr.Instance.Load_Prefab("PlayerBaseAttack");
        //}
        //if (Time.time > (lastFireTime + fireCD) ) {
        //    lastFireTime = Time.time;
        //    Vector2 playerPos = gameObject.transform.position;
        //    playerPos.y = playerPos.y + fixY;
        //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    float angle = Vector2.Angle(Vector2.right, mousePos - playerPos);
        //    angle = mousePos.y > playerPos.y ? angle : -angle;
        //    //UbhObjectPool.Instance.GetGameObject(leftClickPrefab, new Vector3(playerPos.x, playerPos.y, 0),
        //    //                                                                Quaternion.Euler(0, 0, angle));
        //    curShot.Shot();
        //    animCtrl.PlayAttack();
        //}
    }

    private void RightHold() {
        if (rightClickPrefab == null) {
            rightClickPrefab = LocalAssetMgr.Instance.Load_Prefab("PlayerHold");
            rightClickGo = UbhObjectPool.Instance.GetGameObject(rightClickPrefab, Vector3.zero, Quaternion.identity);
        }

        if (rightClickGo == null) {
            Debug.LogError("right go is null");
            return;
        }
        rightClickGo.SetActive(true);

        Vector2 playerPos = gameObject.transform.position;
        playerPos.y = playerPos.y + fixY;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Vector2.Angle(Vector2.right, mousePos - playerPos);
        angle = mousePos.y > playerPos.y ? angle : -angle;

        rightClickGo.transform.position = new Vector3(playerPos.x, playerPos.y, 0);
        rightClickGo.transform.rotation = Quaternion.Euler(0, 0, angle);

        CurMp -= rightCost * Time.deltaTime;
    }

    private void RightRelease() {
        if (rightClickGo == null)
            return;
        rightClickGo.SetActive(false);
    }

    protected override void HitCheck(Transform colTrans) {
        int colLayer = colTrans.gameObject.layer;
        if (colLayer == GeneralDefine.EnemyBulletLayer) {
            //UbhObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);
            Injury();
        }
        if (colLayer == GeneralDefine.TransferLayer) {
            Doorway doorway = colTrans.GetComponent<Transfer>().doorWay;
            if (doorway != null) {
                Send.SendMsg(SendType.Transfer, doorway);
            }
            else {
                Debug.LogError("doorway is null");
            }
        }

        if (colLayer == GeneralDefine.EndGameLayer) {
            WindowMgr.Instance.OpenWindow<ResultWindow>();
        }
    }

    private void MpRecoveryHandle() {
        if (isRightClick)
            return;

        CurMp += mpRecovery * Time.deltaTime;
    }

    protected override void DeadHandle() {
        base.DeadHandle();
        isRightClick = false;
        RightRelease();

        WindowMgr.Instance.OpenWindow<ResultWindow>();
    }

    protected override void Injury(int damageValue = 1) {
        base.Injury(damageValue);
        injuryWeapon.Shot();
    }
}
