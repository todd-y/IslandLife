using UnityEngine;
using System.Collections;

public class BattleGrid : MonoBehaviour {
    private float posX;
    private float posY;
    private float speedX = 500;
    private float speedY = 500;
    public int xIndex;
    public int yIndex;

    public void SetPos(int _xIndex, int _yIndex) {
        xIndex = _xIndex;
        yIndex = _yIndex;

        posX = BattleGridCtrl.arrPosX[xIndex];
        posY = BattleGridCtrl.arrPosY[yIndex];
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

        SetGoPos();
    }

    private void SetGoPos() {
        transform.localPosition = new Vector3(posX, posY, 0);
    }
}
