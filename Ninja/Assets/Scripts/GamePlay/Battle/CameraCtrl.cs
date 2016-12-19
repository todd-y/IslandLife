using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    private System.Action moveEndCallBack;

	// Use this for initialization
	void Start () {
        BattleMgr.Instance.curCameraCtrl = this;
        Send.RegisterMsg(SendType.EnterRoom, OnEnterRoom);
        Send.RegisterMsg(SendType.PlayerPosChange, OnPlayerPosChange);
    }

    void Destroy() {
        Send.UnregisterMsg(SendType.EnterRoom, OnEnterRoom);
        Send.UnregisterMsg(SendType.PlayerPosChange, OnPlayerPosChange);
    }

    public void SetPos(Vector3 newPos, System.Action _callBack) {
        moveEndCallBack = _callBack;
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(newPos.x, newPos.y, transform.position.z), "time", 0.5f, "islocal", true, "oncomplete", "MoveEnd"));
        //transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    private void MoveEnd() {
        if (moveEndCallBack != null) {
            moveEndCallBack();
        }
    }

    private void OnEnterRoom(object[] objs) {
    }

    private void OnPlayerPosChange(object[] objs) {

    }
}
