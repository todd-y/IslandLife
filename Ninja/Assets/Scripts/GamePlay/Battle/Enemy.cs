using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Actor {
    public Transform target;
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
        target = UbhUtil.GetTransformFromTagName("Player");
        ai = new EnemyAI(this);
        ai.IsAwake(false);
        SetBasicInfo(10);
    }

    protected override void HitCheck(Transform colTrans) {
        int colLayer = colTrans.gameObject.layer;
        if (colLayer == GeneralDefine.PlayerBulletLayer) {
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
        animCtrl.PlayAttack();
    }

    public bool CanShot() {
        if(curShot == null)
            return true;
        return !curShot.Shooting;
    }
}
