using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Actor {
    public Transform target;
    [HideInInspector]
    public List<UbhBaseShot> shotList = new List<UbhBaseShot>();
    private UbhBaseShot curShot;

    [HideInInspector]
    public EnemyAI ai;
    [HideInInspector]
    public RoomInfo roomInfo;

    public Enemy() {
        roleType = RoleType.Monster;
    }

    public void Init(RoomInfo _roomInfo) {
        roomInfo = _roomInfo;
    }

    void Update() {
        if (ai == null)
            return;

        ai.Update();
    }

    protected override void BirthHandle() {
        base.BirthHandle();
        UbhBaseShot[] arrayShot = GetComponentsInChildren<UbhBaseShot>();
        for (int index = 0; index < arrayShot.Length; index++ ) {
            arrayShot[index]._BulletPrefab.GetComponent<UbhBullet>().SetColor(Color.red);
        }
        shotList = new List<UbhBaseShot>(arrayShot);
        target = UbhUtil.GetTransformFromTagName("Player");
        ai = new EnemyAI(this);
        ai.IsAwake(false);
        SetBasicInfo(50);
    }

    protected override void HitCheck(Transform colTrans) {
        int colLayer = colTrans.gameObject.layer;
        if (colLayer == GeneralDefine.PlayerBulletLayer) {
            BattleMgr.Instance.curRoom.Draw(colTrans.localPosition, "branch3");
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);
            Injury();
        }
    }

    protected override void DeadHandle() {
        base.DeadHandle();
        ai.IsAwake(false);
        if (curShot != null) curShot.FinishedShot();
        Send.SendMsg(SendType.MonsterDead, this);
    }

    public void Shot(int index) {
        if (curShot!= null && curShot.Shooting) {
            return;
        }
        curShot = shotList[index];
        curShot.Shot();
    }

    public bool CanShot() {
        if (shotList.Count == 0)
            return false;

        if(curShot == null)
            return true;

        return !curShot.Shooting;
    }
}
