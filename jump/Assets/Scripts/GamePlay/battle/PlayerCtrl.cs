﻿using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
    private const float maxPosY = 430;
    private const float minPosY = 130;
    private const float absGavity = 2000;
    private const float backSpeed = 1000;
    private const float speedX = 800;
    private float posX;
    private float posY;
    private float curSpeedY;
    private float gavity;
    private int xIndex;

    void Awake() {
        SetDefaultValue();
    }

    void FixedUpdate() {
        PositionUpdate();
    }

    void Update() {
        InputHandle();
    }

    private void SetDefaultValue() {
        xIndex = 3;
        posX = BattleGridCtrl.arrPosX[xIndex];
        posY = maxPosY;
        curSpeedY = 0;
        gavity = -absGavity;
        SetGoPos();
    }

    private void SetGoPos() {
        transform.localPosition = new Vector3(posX, posY, 0);
    }

    private void PositionUpdate() {
        curSpeedY += gavity * Time.fixedDeltaTime;
        posY += curSpeedY * Time.fixedDeltaTime;

        if (posY <= minPosY) {
            posY = minPosY;
            curSpeedY = backSpeed;

            Send.SendMsg(SendType.PlayerHit, xIndex);
        }

        if (posY >= maxPosY) {
            posY = maxPosY;
        }

        float targetPosX = BattleGridCtrl.arrPosX[xIndex];
        if (targetPosX != posX) {
            bool leftMove = targetPosX < posX;
            posX += speedX * Time.fixedDeltaTime * (leftMove ? -1 : 1);

            if (leftMove) {
                if (posX <= targetPosX) {
                    posX = targetPosX;
                }
            }
            else {
                if (posX >= targetPosX) {
                    posX = targetPosX;
                }
            }
        }

        SetGoPos();
    }

    private void InputHandle() {
        //move
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            xIndex = Mathf.Max(0, xIndex - 1);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            xIndex = Mathf.Min(BattleGridCtrl.xCount - 1, xIndex + 1);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space)) {
        }
        //skill
        if (Input.GetKeyDown(KeyCode.Q)) {
            BattleMgr.Instance.makeInfo.AddItem(2000, 1);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            BattleMgr.Instance.makeInfo.AddItem(2000, 3);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            BattleMgr.Instance.makeInfo.AddItem(2001, 2);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            BattleMgr.Instance.makeInfo.AddItem(2002, 3);
        }

        //item
        if (Input.GetKeyDown(KeyCode.Alpha1)) {

            BattleMgr.Instance.playerInfo.Equip(new EquipData(1004));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {

        }
    }
}
