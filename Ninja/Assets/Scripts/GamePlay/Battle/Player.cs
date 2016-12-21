using UnityEngine;
using System.Collections;
using DunGen;

public class Player : Actor {
    public float fixY = 1;
    public UbhBaseShot curShot;

    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private GameObject rightClickGo;

    private Vector2 tempVector2 = Vector2.zero;
    private bool isRightClick = false;

    private float rightCost = 20;
    private float mpRecovery = 20;
    private float minRightTime = 1;
    private float rightClickTime = 0;
    private Weapeon leftWeapeon;
    private Weapeon injuryWeapon;
    private BackAttack rightWeapeon;
    private Thruster leftThruster;
    private Thruster rightThruster;

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


        SetBodyColor(Color.green);
        SetTargetColor(Color.red);
    }

    private void LoadWeapeon() {
        GameObject leftGo = GameObject.Instantiate(LocalAssetMgr.Instance.Load_Prefab("GunLiner"));
        leftGo.transform.SetParent(transform, false);        
        leftWeapeon = leftGo.GetComponent<Weapeon>();
        leftWeapeon._BulletPrefab.GetComponent<UbhBullet>().SetColor(Color.green);

        GameObject injuryGo = GameObject.Instantiate(LocalAssetMgr.Instance.Load_Prefab("GunCircle"));
        injuryGo.transform.SetParent(transform, false);
        injuryWeapon = injuryGo.GetComponent<Weapeon>();
        injuryWeapon._BulletPrefab.GetComponent<UbhBullet>().SetColor(Color.green);

        rightClickGo = UbhObjectPool.Instance.GetGameObject(LocalAssetMgr.Instance.Load_Prefab("PlayerHold"), Vector3.zero, Quaternion.identity );
        rightWeapeon = rightClickGo.GetComponent<BackAttack>();
        rightWeapeon.SetColor(Color.green, transform);

        leftThruster = gameObject.GetChildControl<Thruster>("LeftThruster");
        rightThruster = gameObject.GetChildControl<Thruster>("RightThruster");
    }

    private void InputHandle() {
        if (alive == false)
            return;

        PlayerMove();
        PlayerHold();
    }

    private void PlayerMove() {
        if (leftThruster != null) {
            leftThruster.Throttle = 1f + 0.5f * Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Horizontal") * 0.5f;
        }

        if (rightThruster != null) {
            rightThruster.Throttle = 1f + 0.5f * Input.GetAxisRaw("Vertical") - Input.GetAxisRaw("Horizontal") * 0.5f;
        }
    }

    private void PlayerHold() {
        float horizontalValue = Input.GetAxisRaw("FireHorizontal");
        float angle = rightWeapeon.Angle - horizontalValue * 180 * Time.deltaTime;
        rightWeapeon.Angle = angle;
        //if (verticalValue != 0) {
        //    angle = (verticalValue > 0 ? 0 : 180) + horizontalValue * (verticalValue > 0 ? -45 : 45);
        //    RightHold(angle);
        //}
        //else if(horizontalValue != 0) {
        //    angle = horizontalValue > 0 ? -90 : 90;
        //    RightHold(angle);
        //}
        //else {
        //    //RightRelease();
        //}
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

    private void RightHold(float _angle) {
        if (rightClickGo == null) {
            Debug.LogError("right go is null");
            return;
        }
        rightClickGo.SetActive(true);

        Vector2 playerPos = gameObject.transform.position;
        playerPos.y = playerPos.y + fixY;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //float angle = Vector2.Angle(Vector2.right, mousePos - playerPos);
        //angle = mousePos.y > playerPos.y ? angle : -angle;
        //angle -= 90;

        rightClickGo.transform.position = new Vector3(playerPos.x, playerPos.y, 0);
        rightClickGo.transform.rotation = Quaternion.Euler(0, 0, _angle);

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
            BattleMgr.Instance.curRoom.Draw(colTrans.localPosition, "branch4");
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

        if (colLayer == GeneralDefine.EndGameLayer) {
            WindowMgr.Instance.OpenWindow<ResultWindow>();
        }

        if (colLayer == GeneralDefine.WallLayer) {
            transform.position = BattleMgr.Instance.curRoom.transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D c) {
        int colLayer = c.gameObject.layer;

        if (colLayer == GeneralDefine.WallLayer) {
            rigidbody2D.AddForceAtPosition( -transform.up * 500, transform.position, ForceMode2D.Force);
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

    protected override void ChangeColor() {
        float per = 0.55f + CurHp / MaxHp * 0.25f;
        colorFX.Desintegration = per;
    }
}
