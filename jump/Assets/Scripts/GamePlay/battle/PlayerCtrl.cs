using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
    private Rigidbody2D body;
    private float addSpeedX = 2000;
    private float reduceSpeedX = 1000;
    private float maxSpeedX = 300;
    private float curSpeedX = 0;
    private float moveFactor = 0;
    private float lastSpeedX;
    private float lastPosY;

    void Awake() {
        InitCtrl();
    }

    public void FixedUpdateHandle() {
        SpeedUpdate();
        PosXUpdate();
    }

    public void UpdateHandle() {
        InputHandle();
        PosYUpdate();
    }

    private void SpeedUpdate() {
        if (moveFactor != 0) {
            curSpeedX += moveFactor * addSpeedX * Time.fixedDeltaTime;
            curSpeedX = Mathf.Clamp(curSpeedX, -maxSpeedX, maxSpeedX);
            lastSpeedX = curSpeedX;
        }
        else if (curSpeedX != 0) {
            curSpeedX = (curSpeedX > 0 ? -reduceSpeedX : reduceSpeedX) * Time.fixedDeltaTime + curSpeedX;
            if (lastSpeedX * curSpeedX <= 0) {
                curSpeedX = 0;
            }
        }
    }

    private void PosXUpdate() {
        if (curSpeedX == 0)
            return;
        transform.localPosition = transform.localPosition + new Vector3(curSpeedX * Time.fixedDeltaTime, 0, 0);
    }

    private void PosYUpdate() {
        float curPosY = transform.localPosition.y;
        if (curPosY - lastPosY != 0) {
            Send.SendMsg(SendType.PlayerYMove, (curPosY - lastPosY));
            lastPosY = curPosY;
        }
    }

    private void InitCtrl() {
        body = gameObject.GetComponent<Rigidbody2D>();

        lastPosY = transform.localPosition.y;
    }

    private void InputHandle() {
        //move
        moveFactor = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            moveFactor = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            moveFactor = 1;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space)) {
        }
        //skill
        if (Input.GetKeyDown(KeyCode.Q)) {
        }
        if (Input.GetKeyDown(KeyCode.W)) {
        }
        if (Input.GetKeyDown(KeyCode.E)) {
        }
        if (Input.GetKeyDown(KeyCode.R)) {
        }

        //item
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
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
