using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Actor {
    public float birthAnimTime = 1.5f;
    public float deadAnimTime = 1f;
    public Transform target;
    [HideInInspector]
    public List<UbhBaseShot> shotList = new List<UbhBaseShot>();
    private UbhBaseShot curShot;

    [HideInInspector]
    public EnemyAI ai;
    [HideInInspector]
    public RoomInfo roomInfo;

    private bool inAlpha = false;
    private float needAlphaTime = 0;
    private float curAlphaTime = 0;
    private float startAlphaValue = 0;
    private float targetAlphaValue = 0;

    private float lastMoveTime = 0;
    private float moveTimeDelay;

    public void Init(RoomInfo _roomInfo) {
        roomInfo = _roomInfo;
    }

    void Update() {
        if (inAlpha) {
            AlphaHandle();
            return;
        }
        MoveHandle();
        if (ai != null) {
            ai.Update();
        }
    }

    private void AlphaHandle() {
        curAlphaTime += Time.deltaTime;
        float curValue = Mathf.Lerp(startAlphaValue, targetAlphaValue, curAlphaTime / needAlphaTime);
        Color curColor = bodyRenderer.color;
        curColor.a = curValue;
        bodyRenderer.color = curColor;
        if (curAlphaTime >= needAlphaTime) {
            inAlpha = false;
            curColor.a = targetAlphaValue;
            bodyRenderer.color = curColor;

            if (targetAlphaValue == 1) {
                SetColliderState(true);
            }
            else {
                UbhObjectPool.Instance.ReleaseGameObject(gameObject);
            }
        }
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
        SetBasicInfo(5);

        StartAlpha(birthAnimTime, 0, 1);
    }

    private void StartAlpha(float _time, float _startValue, float _targetValue) {
        if (_time == 0 || bodyRenderer == null)
            return;
        SetColliderState(false);
        Color nowColor = bodyRenderer.color;
        nowColor.a = _startValue;
        bodyRenderer.color = nowColor;
        inAlpha = true;
        curAlphaTime = 0;
        needAlphaTime = _time;
        startAlphaValue = _startValue;
        targetAlphaValue = _targetValue;
    }

    protected override void HitCheck(Transform colTrans) {
        int colLayer = colTrans.gameObject.layer;
        if (colLayer == GeneralDefine.PlayerBulletLayer) {
            BattleMgr.Instance.curRoom.Draw(colTrans.localPosition, "branch3");
            rigidbody2D.AddForceAtPosition(colTrans.up * 10, colTrans.position, ForceMode2D.Force);
            BattleMgr.Instance.curCameraCtrl.SetShake(0.2f);
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);
            Injury();
            SoundManager.Instance.PlaySound("hit");
        }
    }

    protected override void DeadHandle() {
        base.DeadHandle();
        ai.IsAwake(false);
        if (curShot != null) curShot.FinishedShot();
        Send.SendMsg(SendType.MonsterDead, this);
        StartAlpha(deadAnimTime, 1, 0);
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

    public void MoveHandle() {
        if (roleType == RoleType.Battlery)
            return;
        if (Time.time - lastMoveTime > moveTimeDelay) {
            lastMoveTime = Time.time;
            float minforce = -200;
            float maxforce = 200;
            rigidbody2D.AddForceAtPosition(new Vector3(Random.Range(minforce, maxforce), Random.Range(minforce, maxforce), 0),
                                            transform.position, ForceMode2D.Force);
            moveTimeDelay = Random.Range(1f, 3f);
        }
    }
}
