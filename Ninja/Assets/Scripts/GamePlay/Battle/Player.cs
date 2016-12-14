using UnityEngine;
using System.Collections;
using DunGen;

public class Player : Actor {
    public float fixY = 1;

    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private GameObject leftClickPrefab;
    private GameObject rightClickPrefab;
    private GameObject rightClickGo;

    private float fireCD = 0.5f;
    private float lastFireTime = 0;
    private Vector2 tempVector2 = Vector2.zero;
    private bool isRightClick = false;

    private float rightCost = 20;
    private float mpRecovery = 20;
    private float minRightTime = 1;
    private float rightClickTime = 0;

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
        if (Input.GetMouseButtonDown(0)) {
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
        if (leftClickPrefab == null) {
            leftClickPrefab = LocalAssetMgr.Instance.Load_Prefab("PlayerBaseAttack");
        }
        if (Time.time > (lastFireTime + fireCD) ) {
            lastFireTime = Time.time;
            Vector2 playerPos = gameObject.transform.position;
            playerPos.y = playerPos.y + fixY;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Vector2.Angle(Vector2.right, mousePos - playerPos);
            angle = mousePos.y > playerPos.y ? angle : -angle;
            UbhObjectPool.Instance.GetGameObject(leftClickPrefab, new Vector3(playerPos.x, playerPos.y, 0),
                                                                            Quaternion.Euler(0, 0, angle));
            animCtrl.PlayAttack();
        }
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
        if (colLayer == GeneralDefine.EnemyBulletLayer || colLayer == GeneralDefine.EnemyLayer) {
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);
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
    }
}
