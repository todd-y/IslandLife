using UnityEngine;
using System.Collections;
using DunGen;

public class Player : Actor {
    public float fixY = 1;

    private float mpRecovery = 0;
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

        GameObject rightGo = UbhObjectPool.Instance.GetGameObject(LocalAssetMgr.Instance.Load_Prefab("PlayerHold"), Vector3.zero, Quaternion.identity);
        rightWeapeon = rightGo.GetComponent<BackAttack>();
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

    private void MpRecoveryHandle() {
        CurMp += mpRecovery * Time.deltaTime;
    }

    protected override void DeadHandle() {
        base.DeadHandle();

        WindowMgr.Instance.OpenWindow<ResultWindow>();
    }

    protected override void Injury(int damageValue = 1) {
        base.Injury(damageValue);
        injuryWeapon.Shot();
    }

    void OnCollisionEnter2D(Collision2D c) {
        int colLayer = c.gameObject.layer;

        if (colLayer == GeneralDefine.WallLayer) {
            rigidbody2D.AddForceAtPosition(-transform.up * 500, transform.position, ForceMode2D.Force);
        }
    }

}
