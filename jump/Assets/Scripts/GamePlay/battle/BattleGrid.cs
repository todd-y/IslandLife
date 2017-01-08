using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleGrid : MonoBehaviour {
    private float posX;
    private float posY;
    private float rotY;
    private float speedX = 500;
    private float speedY = 500;
    private float speedRotY = 500;
    private float targetRotY;
    private Color hideColor = new Color(118f / 255, 118f / 255, 118f / 255);
    private Color showColor = new Color(218f / 255, 218f / 255, 218f / 255);
    public int xIndex;
    public int yIndex;

    private Image imgIcon;
    private Image imgBg;

    private State m_curState;
    public State CurState {
        get {
            return m_curState;
        }
        set {
            m_curState = value;
            if (m_curState == State.Hide) {
                imgIcon.enabled = false;
                imgBg.color = hideColor;
                targetRotY = 180;

            }
            else {
                targetRotY = 0;
            }
        }
    }

    public void InitPos(int _xIndex, int _yIndex) {
        imgBg = gameObject.GetChildControl<Image>("imgBg");
        imgIcon = gameObject.GetChildControl<Image>("imgIcon");

        xIndex = _xIndex;
        yIndex = _yIndex;

        posX = BattleGridCtrl.arrPosX[xIndex];
        posY = BattleGridCtrl.arrPosY[yIndex];
        CurState = State.Hide;
        rotY = targetRotY;

        SetGoPos();
    }

    public void SetData() {

    }

    public void SetTargetPos(int _xIndex, int _yIndex) {
        xIndex = _xIndex;
        yIndex = _yIndex;
    }

    void FixedUpdate() {
        PositionUpdate();
	}

    private void PositionUpdate() {
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

        float targetPosY = BattleGridCtrl.arrPosY[yIndex];
        if (targetPosY != posY) {
            bool downMove = targetPosY < posY;
            posY += speedY * Time.fixedDeltaTime * (downMove ? -1 : 1);

            if (downMove) {
                if (posY <= targetPosY) {
                    posY = targetPosY;
                }
            }
            else {
                if (posY >= targetPosY) {
                    posY = targetPosY;
                }
            }
        }

        if (targetRotY != rotY) {
            bool waitChange = rotY > 90;
            rotY -= speedRotY * Time.fixedDeltaTime;
            if (waitChange && rotY <= 90) {
                imgIcon.enabled = true;
                imgBg.color = showColor;
            }

            if (rotY < targetRotY) {
                rotY = targetRotY;
            }
        }

        SetGoPos();
    }

    private void SetGoPos() {
        transform.localPosition = new Vector3(posX, posY, 0);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotY, transform.eulerAngles.z);
    }

    public enum State {
        Hide,
        Show,
    }
}
